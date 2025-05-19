using System.IdentityModel.Tokens.Jwt;
using System.IO.IsolatedStorage;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.IO.IsolatedStorage;


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
        signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)

    );

    public static TokenValidationParameters
        GetTokenValidationParameters() =>


        new TokenValidationParameters
        {
            ValidIssuer = issuer,
            ValidAudience = issuer,
            IssuerSigningKey = Key,
            ClockSkew = TimeSpan.FromMinutes(5),
        };

    public static string WriteToken(SecurityToken token) =>
        new JwtSecurityTokenHandler().WriteToken(token);
 public static ClaimsPrincipal ValidateToken(string token)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    try
    {
        // Validate the token using the parameters שהוגדרו
        var principal = tokenHandler.ValidateToken(token, GetTokenValidationParameters(), out var validatedToken);

        // אם הטוקן תקף, מחזירים את ה-ClaimsPrincipal
        return principal;
    }
    catch (Exception ex)
    {
        throw new SecurityTokenException("Invalid token", ex);
    }
} 


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

    public static void SaveToken(string token)
    {
        using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
        {
            using (
                var stream = new IsolatedStorageFileStream("token.txt", FileMode.Create, storage)
            )
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(token);
            }
        }
    }

    public static string LoadToken()
    {
        using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
        {
            if (storage.FileExists("token.txt"))
            {
                using (
                    var stream = new IsolatedStorageFileStream("token.txt", FileMode.Open, storage)
                )
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        return string.Empty;
    }

    internal static bool IsTokenValid(string token)
    {
        throw new NotImplementedException();
    }
}