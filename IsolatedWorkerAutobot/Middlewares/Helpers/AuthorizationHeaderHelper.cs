using IsolatedWorkerAutobot.Constants;

namespace IsolatedWorkerAutobot.Middlewares.Helpers;

internal static class AuthorizationHeaderHelper
{
    public static async Task<bool> Verify(FunctionContext context)
    {
        var requestData = await context.GetHttpRequestDataAsync();
        var token = requestData.Query.Get(AuthCode.Token);

        return true;
    }
}
