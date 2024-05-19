using Application.Services;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Presentation.Services;


public sealed class TokenService : ITokenService
{
    private readonly byte[] TokenKey;

    public TokenService(IOptions<Options> options)
    {
        TokenKey = options.Value.TokenKey;
    }

    public string GetToken(Player user)
    {
        var claims = new HashSet<Claim> {
            new(ClaimTypes.NameIdentifier, user.Id!.ToString()!)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(16),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(TokenKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters()
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = new SymmetricSecurityKey(TokenKey)
        };

        try
        {
            return tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
        }
        catch (SecurityTokenException)
        {

            return null;
        }
    }

    public class Options
    {
        public byte[] TokenKey { get; set; } = Array.Empty<byte>();
    }
}
