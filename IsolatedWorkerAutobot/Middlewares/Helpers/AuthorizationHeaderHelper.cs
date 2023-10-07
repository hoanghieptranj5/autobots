using Microsoft.Azure.Functions.Worker;

namespace IsolatedWorkerAutobot.Middlewares.Helpers;

internal static class AuthorizationHeaderHelper
{
  public static async Task<bool> Verify(FunctionContext context)
  {
    const string key = "code";
    var requestData = await context.GetHttpRequestDataAsync();
    var token = requestData.Query.Get(key);

    return true;
  }
}