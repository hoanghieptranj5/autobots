using System.Net;
using System.Net.Http.Json;
using IAM.Data.Abstraction;
using IAM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace IsolatedWorkerAutobot.Functions;

public class UserCrud
{
  private readonly IUserService _userService;
  private readonly ILogger _logger;

  public UserCrud(IUserService userService, ILoggerFactory loggerFactory)
  {
    _userService = userService;
    _logger = loggerFactory.CreateLogger<HttpExample>();
  }

  // 👇👇👇👇👇 Add OpenAPI related decorators below 👇👇👇👇👇
  [OpenApiOperation(operationId: "AddSingleUser", tags: new[] { "AddSingleUser" }, Summary = "AddSingleUser",
    Description = "This adds a new user to system.", Visibility = OpenApiVisibilityType.Important)]
  [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
  [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateUserRequest), Description = "Parameters", Example = typeof(CreateUserRequest))]
  [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string),
    Summary = "The response", Description = "This returns the response")]
  // 👆👆👆👆👆 Add OpenAPI related decorators above 👆👆👆👆👆
  [Function("AddSingleUser")]
  public async Task<IActionResult> AddSingleUser(
    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user")] HttpRequestData req)
  {
    _logger.LogInformation("HTTP trigger function create new user");
    var msg = await req.ReadAsStringAsync();
    var creatUserRequest = JsonConvert.DeserializeObject<CreateUserRequest>(msg);

    var result = await _userService.CreateSingle(creatUserRequest);
    return new OkObjectResult(result);
  }
  
  // 👇👇👇👇👇 Add OpenAPI related decorators below 👇👇👇👇👇
  [OpenApiOperation(operationId: "DeleteSingleUser", tags: new[] { "DeleteSingleUser" }, Summary = "DeleteSingleUser",
    Description = "This remove a user of system.", Visibility = OpenApiVisibilityType.Important)]
  [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
  [OpenApiParameter("username", Type = typeof(string))]
  [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string),
    Summary = "The response", Description = "This returns the response")]
  // 👆👆👆👆👆 Add OpenAPI related decorators above 👆👆👆👆👆
  [Function("DeleteSingleUser")]
  public async Task<IActionResult> DeleteSingleUser(
    [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "user/username/{username}")] HttpRequestData req, string username)
  {
    _logger.LogInformation($"HTTP trigger function delete a user with username {username}");
    try
    {
      var result = await _userService.DeleteSingleByUsername(username);

      return new OkObjectResult(result);
    }
    catch (Exception ex)
    {
      return new BadRequestObjectResult(ex.Message);
    }
  }
}