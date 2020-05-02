using Pulumi;
using Pulumi.Azure.ApiManagement.Outputs;
using Pulumi.Azure.AppInsights;
using Pulumi.Azure.AppService;
using Pulumi.Azure.AppService.Inputs;
using Pulumi.Azure.AppService.Outputs;
using Pulumi.Azure.Core;
using Pulumi.Azure.CosmosDB;
using Pulumi.Azure.CosmosDB.Inputs;
using Pulumi.Azure.Monitoring.Outputs;
using Pulumi.Azure.Network.Outputs;
using Pulumi.Azure.Storage;
using System;

class TracinhaStack : Stack
{
    public TracinhaStack()
    {
        var config = new Config();

        // Create an Azure Resource Group
        var resourceGroup = new ResourceGroup("tracinha-rg", new ResourceGroupArgs()
        {
            Name = "tracinha-rg",
            Location = "brazilsouth",
        });

        // Create an AppService Plan
        var appServicePlan = CreateAppServicePlan(resourceGroup);

        // Create an Azure Storage Account
        var storageAccount = CreateStorageAccount(resourceGroup);

        // Create a Storage Container
        var container = CreateStorageContainer(storageAccount);

        // Create a Blob to store the App
        var blob = CreateBlob(storageAccount, container);

        // Create SAS key
        var codeBlobUrl = SharedAccessSignature.SignedBlobReadUrl(blob, storageAccount);

        // Create a CosmosDB Account
        var cosmosAccount = new Pulumi.Azure.CosmosDB.Account("tracinha", new Pulumi.Azure.CosmosDB.AccountArgs()
        {
            GeoLocations = new AccountGeoLocationArgs()
            {
                Location = "brazilsouth",
                FailoverPriority = 0
            },
            ConsistencyPolicy = new AccountConsistencyPolicyArgs()
            {
                ConsistencyLevel = "session",
            },
            OfferType = "Standard",
            ResourceGroupName = resourceGroup.Name,
            Location = "brazilsouth"
        });

        var database = new SqlDatabase("Tracinha", new SqlDatabaseArgs()
        {
            ResourceGroupName = resourceGroup.Name,
            AccountName = cosmosAccount.Name,
            Throughput = 400
        });

        var bookshelfCollection= new SqlContainer("Bookshelf", new SqlContainerArgs()
        {
            ResourceGroupName = resourceGroup.Name,
            AccountName = cosmosAccount.Name,
            DatabaseName = database.Name,
            PartitionKeyPath = "/userId"
        });

        var reviewContainer = new SqlContainer("Reviews", new SqlContainerArgs()
        {
            ResourceGroupName = resourceGroup.Name,
            AccountName = cosmosAccount.Name,
            DatabaseName = database.Name,
            PartitionKeyPath = "/bookId"
        });

        // Create Application Insights
        var appInsights = new Insights("tracinha-ins", new InsightsArgs()
        {
            ResourceGroupName = resourceGroup.Name,
            Location = "brazilsouth",
            ApplicationType = "web"
        });

        // Configure AppSettings
        var appSettings = new InputMap<string>()
            {
{ "runtime", "dotnet" },
{ "WEBSITE_RUN_FROM_PACKAGE", codeBlobUrl },
            { "apiKey", config.Require("apiKey") },
            { "CosmosDB", cosmosAccount.Endpoint },
            { "APPINSIGHTS_INSTRUMENTATIONKEY", appInsights.InstrumentationKey },
};

        // Create the FunctionApp
        var function = CreateFunctionApp(config, resourceGroup, appServicePlan, storageAccount, appSettings);

        // Export the connection string for the storage account
        this.Endpoint = function.DefaultHostname;
    }

    private FunctionApp CreateFunctionApp(Config config, ResourceGroup resourceGroup, Plan appServicePlan, Pulumi.Azure.Storage.Account storageAccount, InputMap<string> appSettings)
    {
        return new FunctionApp("tracinha-functions", new FunctionAppArgs()
        {
            Name = "tracinha-functions",
            AppServicePlanId = appServicePlan.Id,
            ResourceGroupName = resourceGroup.Name,
            StorageAccountName = storageAccount.Name,
            StorageAccountAccessKey = storageAccount.PrimaryAccessKey,
            OsType = "linux",
            Version = "~3",
            AppSettings = appSettings,
            AuthSettings = new FunctionAppAuthSettingsArgs()
            {
                AllowedExternalRedirectUrls = new string[] { "tracinha://" },
                Enabled = true,
                UnauthenticatedClientAction = "AllowAnonymous",
                Google = new FunctionAppAuthSettingsGoogleArgs()
                {
                    ClientId = config.RequireSecret("Google_Client_Id"),
                    ClientSecret = config.RequireSecret("Google_Client_Secret"),
                    OauthScopes = config.RequireSecret("Google_Scopes").ToString().Split(',')
                }
            }
        });
    }

    private Blob CreateBlob(Pulumi.Azure.Storage.Account storageAccount, Container container)
    {
        return new Blob("zip", new BlobArgs()
        {
            StorageAccountName = storageAccount.Name,
            StorageContainerName = container.Name,
            Source = new FileArchive(@"..\Artifacts\Functions"),
            Type = "Block"
        });
    }

    private Container CreateStorageContainer(Pulumi.Azure.Storage.Account storageAccount)
    {
        return new Container("zips", new ContainerArgs()
        {
            StorageAccountName = storageAccount.Name,
            ContainerAccessType = "private"
        });
    }

    private Pulumi.Azure.Storage.Account CreateStorageAccount(ResourceGroup resourceGroup)
    {
        return new Pulumi.Azure.Storage.Account("tracinhast", new Pulumi.Azure.Storage.AccountArgs
        {
            Name = "tracinhast",
            ResourceGroupName = resourceGroup.Name,
            AccountReplicationType = "LRS",
            AccountTier = "Standard"
        });
    }

    private Plan CreateAppServicePlan(ResourceGroup resourceGroup)
    {
        return new Plan("tracinha-plan", new PlanArgs()
        {
            Name = "tracinha-plan",
            Kind = "FunctionApp",
            ResourceGroupName = resourceGroup.Name,
            Location = "brazilsouth",
            Sku = new PlanSkuArgs()
            {
                Tier = "Dynamic",
                Size = "Y1",
            },
        });
    }

    [Output]
    public Output<string> ConnectionString { get; private set; }
    public Output<string> Endpoint { get; private set; }
}
