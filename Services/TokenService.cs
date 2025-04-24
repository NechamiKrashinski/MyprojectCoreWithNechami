using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace project.Services;

public static class TokenService
{
    private static SymmetricSecurityKey Key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes("111555zbzhcC5fvyGfeifzxjc58zxvyaxfGGjilllLLMBGJH"));
    
    private static string issuer = "http://localhost:5172";
    public static SecurityToken GetToken(List<Claim> claims) => new JwtSecurityToken(   
        issuer,
        issuer,
        claims,
        expires: DateTime.Now.AddDays(30.0),
        signingCredentials: new SigningCredentials(Key,SecurityAlgorithms.HmacSha256)

    );

    public static TokenValidationParameters
        GetTokenValidationParameters()=>
        new TokenValidationParameters
        {
            ValidIssuer = issuer,
            ValidAudience = issuer,
            IssuerSigningKey = Key,
            ClockSkew = TimeSpan.Zero,
        };
    public static string WriteToken(SecurityToken token) =>
        new JwtSecurityTokenHandler().WriteToken(token);
}