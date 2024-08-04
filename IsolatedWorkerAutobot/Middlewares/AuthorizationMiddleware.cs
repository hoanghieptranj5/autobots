using IsolatedWorkerAutobot.Constants;
using IsolatedWorkerAutobot.Exceptions;
using IsolatedWorkerAutobot.Middlewares.Helpers;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace IsolatedWorkerAutobot.Middlewares;

public class AuthorizationMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ILogger<AuthorizationMiddleware> _logger;

    public AuthorizationMiddleware(ILogger<AuthorizationMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var targetMethod = TargetMethodHelper.GetTargetFunctionMethod(context);
        var authAttrs = TargetMethodHelper.GetFunctionMethodAttribute<AuthorizeAttribute>(targetMethod);
        var allowAllAttrs = TargetMethodHelper.GetFunctionMethodAttribute<AllowAnonymousAttribute>(targetMethod);

        if (authAttrs.Any())
        {
            _logger.LogDebug($"This method {targetMethod.Name} requires authorization.");

            var requestData = await context.GetHttpRequestDataAsync();
            var bearerString = requestData.Headers.GetValues(AuthCode.Token).FirstOrDefault();

            if (bearerString == null || !bearerString.StartsWith("Bearer "))
            {
                const string message =
                    "Token is missing the Bearer prefix.";
                await MiddlewareResponseWriter.WriteAsJsonAsync(context, message, HttpStatusCode.BadRequest);
                return;
            }

            var token = bearerString.Replace("Bearer ", string.Empty);

            try
            {
                JwtTokenValidator.Verify(token);
            }
            catch (TokenUnavailableException ex)
            {
                const string message =
                    "Access to this endpoint is unauthorized due to an invalid or expired JSON Web Token (JWT).";
                await MiddlewareResponseWriter.WriteAsJsonAsync(context, message, HttpStatusCode.Unauthorized);
                return;
            }
            catch (TokenExpiredException ex)
            {
                const string message =
                    "Token has been expired. Please try to login again or use the refresh token instead.";
                await MiddlewareResponseWriter.WriteAsJsonAsync(context, message, HttpStatusCode.Unauthorized);
            }
            catch (Exception ex)
            {
                var message = $"Unexpected error when validating token: {ex.Message}";
                await MiddlewareResponseWriter.WriteAsJsonAsync(context, message, HttpStatusCode.InternalServerError);
            }
        }
        else if (allowAllAttrs.Any())
        {
            _logger.LogDebug($"This method {targetMethod.Name} allow all inbounds.");
        }

        await next(context);
    }
}
