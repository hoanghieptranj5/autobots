using IAM.DI;
using IsolatedWorkerAutobot.Middlewares;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repositories.Models;
using Repositories.UnitOfWork;
using Repositories.UnitOfWork.Abstractions;

var host = new HostBuilder()
  .ConfigureFunctionsWorkerDefaults(worker =>
  {
    worker.UseMiddleware<AuthorizationMiddleware>();
    worker.UseNewtonsoftJson();
  })
  .ConfigureOpenApi()
  .ConfigureServices(services =>
  {
    var connectionString =
      Environment.GetEnvironmentVariable("SqlConnectionString", EnvironmentVariableTarget.Process);
    services.AddDbContext<ApplicationDbContext>(options =>
      SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString));

    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.SetupIAMDependencies();
    services.AddEndpointsApiExplorer();
  })
  .Build();

host.Run();