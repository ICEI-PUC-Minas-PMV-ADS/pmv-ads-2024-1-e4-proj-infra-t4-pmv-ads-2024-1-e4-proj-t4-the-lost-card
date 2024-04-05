using Application.Services;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;

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
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        if (user.CurrentRoom is not null)
            claims.Add(new Claim(ClaimTypes.GroupSid, user.CurrentRoom!.ToString()!));

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(TokenKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public class Options
    {
        public byte[] TokenKey { get; set; }
    }
}
