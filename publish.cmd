@SET ARTIFACTS_WORKSPACE=%CD%
@dotnet publish -c Release -o DEVOPS\Artifacts\Functions Functions\Xamarin.Bookshelf.Functions\Xamarin.Bookshelf.Functions.csproj
@pushd .
@cd DEVOPS\Xamarin.Bookshelf.Infrastructure
@pulumi up -y
@popd