using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace IAM.Helper;

public static class JwtHelper
{
  private const string Secret = "ThisIsASuperMegaUltimateLongPasswordWithSpecialChar%^$@*";
  private const string Issuer = "http://mysite.com";
  private const string Audience = "http://myaudience.com";
  private static SymmetricSecurityKey SecurityKey = new(Encoding.ASCII.GetBytes(Secret));

  public static string GenerateToken(int userId, string email)
  {
    var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));

    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new Claim[]
      {
        new(ClaimTypes.NameIdentifier, userId.ToString()),
        new(ClaimTypes.Email, email)
      }),
      Expires = DateTime.UtcNow.AddDays(7),
      Issuer = Issuer,
      Audience = Audience,
      SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }

  public static bool ValidateToken(string token)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    try
    {
      tokenHandler.ValidateToken(token, new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = Issuer,
        ValidAudience = Audience,
        IssuerSigningKey = SecurityKey
      }, out var validatedToken);
    }
    catch
    {
      return false;
    }

    return true;
  }

  public static string GetClaim(string token, string claimType)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

    var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType)?.Value;
    return stringClaimValue ?? "N/A";
  }
}