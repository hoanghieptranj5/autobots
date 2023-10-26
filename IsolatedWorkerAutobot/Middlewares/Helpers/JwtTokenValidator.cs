using System.Security.Claims;
using IAM.Helper;
using IsolatedWorkerAutobot.Exceptions;

namespace IsolatedWorkerAutobot.Middlewares.Helpers;

internal static class JwtTokenValidator
{
    #region Public Methods

    /// <summary>
    ///     Three steps verifications
    /// </summary>
    /// <param name="token"></param>
    /// <exception cref="TokenUnavailableException"></exception>
    /// <exception cref="TokenExpiredException"></exception>
    public static void Verify(string token)
    {
        if (!IsTokenAvailable(token)) throw new TokenUnavailableException("Token is missing or invalid format.");

        var expiryTime = GetExpiryTime(token);

        if (IsTokenExpired(expiryTime)) throw new TokenExpiredException("Token has expired.");
    }

    #endregion

    #region Private Methods

    private static bool IsTokenAvailable(string token)
    {
        return !string.IsNullOrEmpty(token) && JwtHelper.ValidateToken(token);
    }

    private static DateTime GetExpiryTime(string token)
    {
        // Extract the expiry time from the JWT token.
        // You can use a JWT library to decode the token and get the expiry claim.
        // For example, in the System.IdentityModel.Tokens.Jwt library:
        // var jwtToken = new JwtSecurityToken(token);
        // return jwtToken.ValidTo;
        // This depends on the library you are using for JWT handling.
        // Make sure to handle any exceptions that may occur during token parsing.
        var expiration = JwtHelper.GetClaim(token, ClaimTypes.Expiration);
        DateTime.TryParse(expiration, out var expirationDateTime);
        return expirationDateTime;
    }

    private static bool IsTokenExpired(DateTime expiryTime)
    {
        // Compare the expiry time with the current time.
        return DateTime.UtcNow >= expiryTime;
    }

    #endregion
}
