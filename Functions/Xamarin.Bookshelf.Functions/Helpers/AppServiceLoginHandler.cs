using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Xamarin.Bookshelf.Functions.Helpers
{
    public static class AppServiceLoginHandler
    {
        public static JwtSecurityToken CreateToken(IEnumerable<Claim> claims, string secretKey, string audience, string issuer, TimeSpan? lifetime)
        {
            if (claims == null)
            {
                throw new ArgumentNullException("cSigninKlaims");
            }

            if (lifetime != null && lifetime < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("lifetime");
            }

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException("secretKey");
            }

            if (claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub) == null)
            {
                throw new ArgumentOutOfRangeException("claims");
            }

            // add the claims passed in
            Collection<Claim> finalClaims = new Collection<Claim>();
            foreach (Claim claim in claims)
            {
                finalClaims.Add(claim);
            }

            // add our standard claims
            finalClaims.Add(new Claim("ver", "3"));
            finalClaims.Add(new Claim("idp", "Apple"));

            return CreateTokenFromClaims(finalClaims, secretKey, audience, issuer, lifetime);
        }

        internal static JwtSecurityToken CreateTokenFromClaims(IEnumerable<Claim> claims, string secretKey, string audience, string issuer, TimeSpan? lifetime)
        {
            DateTime created = DateTime.UtcNow;

            // we allow for no expiry (if lifetime is null)
            DateTime? expiry = (lifetime != null) ? created + lifetime : null;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = audience,
                Issuer = issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest),
            Expires = expiry,
                NotBefore = created,
                IssuedAt = created,
                Subject = new ClaimsIdentity(claims),
            };

            var securityTokenHandler = new JwtSecurityTokenHandler();
            return securityTokenHandler.CreateToken(tokenDescriptor) as JwtSecurityToken;
        }
    }
}
