using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Xamarin.Bookshelf.Functions.Helpers;
using Xamarin.Bookshelf.Shared;
using Xamarin.Bookshelf.Shared.Models;

namespace Xamarin.Bookshelf.Functions
{
    public class SigninWithAppleFunctions
    {
        private readonly string _host = string.Format("https://{0}.azurewebsites.net/",
            Environment.ExpandEnvironmentVariables("%WEBSITE_SITE_NAME%")
            .ToLower());

        [FunctionName("SigninWithApple")]
        public async Task<IActionResult> SigninWithApple(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = ApiRoutes.API_SIGNIN_WITH_APPLE)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function started.");
            var response = await new StreamReader(req.Body).ReadToEndAsync();
            var source = JsonConvert.DeserializeObject<AppleAuthenticationResult>(response);
            Claim[] claims = GenerateClaims(source);

            var result = AppServiceLoginHandler.CreateToken(claims,
                Environment.GetEnvironmentVariable("WEBSITE_AUTH_SIGNING_KEY"),
                _host,
                _host,
                TimeSpan.FromHours(24));

            return new OkObjectResult(result.RawData);
        }

        private Claim[] GenerateClaims(AppleAuthenticationResult source)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, source.UserId));
            if (!String.IsNullOrWhiteSpace(source.Name))
            {
                claims.Add(new Claim(ClaimTypes.Name, source.Name));
            }

            if (!string.IsNullOrWhiteSpace(source.Email))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, source.Email));
            }
            return claims.ToArray();
        }
    }
}