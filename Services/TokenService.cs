using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using project.Models;

namespace project.Services;

public static class TokenService
{
    private static SymmetricSecurityKey Key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes("111555zbzhcC5fvyGfeifzxjc58zxvyaxfGGjilllLLMBGJH")
    );

    private static string issuer = "http://localhost:5172";

    public static SecurityToken GetToken(List<Claim> claims) =>
        new JwtSecurityToken(
            issuer,
            issuer,
            claims,
            expires: DateTime.Now.AddDays(30.0),
            signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)
        );

    public static TokenValidationParameters GetTokenValidationParameters() =>
        new TokenValidationParameters
        {
            ValidIssuer = issuer,
            ValidAudience = issuer,
            IssuerSigningKey = Key,
            ClockSkew = TimeSpan.Zero,
        };

    public static string WriteToken(SecurityToken token) =>
        new JwtSecurityTokenHandler().WriteToken(token);

    public static ClaimsPrincipal DecodeToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (principal == null)
                throw new SecurityTokenException("Invalid token");

            return new ClaimsPrincipal(new ClaimsIdentity(principal.Claims));
        }
        catch (Exception ex)
        {
            throw new SecurityTokenException("Token decoding failed", ex);
        }
    }

    public static bool IsTokenValid(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(
                token,
                GetTokenValidationParameters(),
                out SecurityToken validatedToken
            );
            return validatedToken != null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static CurrentUser GetCurrentUser(string token)
    {
        var claims = DecodeToken(token);

        CurrentUser currentUser = new CurrentUser();
        currentUser.Id = int.Parse(claims.FindFirst(c => c.Type == "Id").Value);
        currentUser.role = (Role)
            Enum.Parse(typeof(Role), claims.FindFirst(c => c.Type == "Role").Value);
        return currentUser;
    }
}
