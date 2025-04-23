
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;


namespace project.Services;


public static class TokenService
{
    private static SymmetricSecurityKey key = 
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                ("hq137cRW#g8h3dGYuuhFhjdsufie89dklscldHVJFG"));
    private static string issuer = "https://localhost:7094";
    public static SecurityToken GetToken(List<Claim> claims)=>
            new JwtSecurityToken(
                issuer,
                issuer,
                claims,
                expires: DateTime.Now.AddDays(30.0),
                signingCredentials: new SigningCredentials(key,SecurityAlgorithms.HmacSha256)
            );
    
    public static TokenValidationParameters GetTokenValidationParameters()=>
    new TokenValidationParameters{
        ValidIssuer  =issuer,
        ValidAudience =issuer,
        IssuerSigningKey = key,
        ClockSkew = TimeSpan.Zero,
    };
    
    public static string WriteToken(SecurityToken token)=>
        new JwtSecurityTokenHandler().WriteToken(token);
}