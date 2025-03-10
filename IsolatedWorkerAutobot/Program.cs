using HanziCollector.Abstraction;
using HanziCollector.DI;
using HanziCollector.Implementations;
using IAM.DI;
using IsolatedWorkerAutobot.Mappers;
using IsolatedWorkerAutobot.Middlewares;
using IsolatedWorkerAutobot.ValuedObjects;
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
            options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICalculationLogic, CalculationLogic>();
        services.AddScoped<IElectricPriceService, ElectricPriceService>();
        services.AddScoped<IVocabularyDbService, VocabularyDbService>();

        services.AddAutoMapper(typeof(VocabularyProfile));

        services.SetupHanziDependencies();
        services.SetupIAMDependencies();
        services.AddEndpointsApiExplorer();
    })
    .Build();

host.Run();
