using CosmosRepository.Clients;
using HanziCollector.DI;
using IAM.DI;
using IsolatedWorkerAutobot.Mappers;
using IsolatedWorkerAutobot.Middlewares;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        services.AddSingleton<CosmosDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddAutoMapper(typeof(VocabularyProfile));

        services.SetupHanziDependencies();
        services.SetupIAMDependencies();
        services.AddEndpointsApiExplorer();
    })
    .Build();

host.Run();
