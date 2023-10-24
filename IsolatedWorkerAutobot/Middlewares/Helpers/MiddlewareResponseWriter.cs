using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace IsolatedWorkerAutobot.Middlewares.Helpers;

public class MiddlewareResponseWriter
{
  public static async Task WriteAsJsonAsync<T>(FunctionContext context, T message, HttpStatusCode statusCode)
  {
    var req = await context.GetHttpRequestDataAsync();
    var res = req!.CreateResponse();

    await res.WriteAsJsonAsync(message, statusCode);

    context.GetInvocationResult().Value = res;
  }
}