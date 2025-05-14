using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shared.DevTools
{
    public static class DevTokenEndpoint
    {
        public static IEndpointRouteBuilder MapDevTokenGenerator(this IEndpointRouteBuilder endpoints, IConfiguration config)
        {
            endpoints.MapGet("/dev/token", () =>
            {
                var secretKey = config["Jwt:SecretKey"];
                var issuer = config["Jwt:Issuer"];
                var audience = config["Jwt:Audience"];

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, "user-123"),
                new Claim("role", "User"), // or RestaurantOwner
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(2),
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Results.Ok(new { token = tokenString });
            });

            return endpoints;
        }
    }

}
