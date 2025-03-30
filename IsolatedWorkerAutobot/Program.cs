using CosmosRepository.Clients;
using CosmosRepository.Contracts;
using CosmosRepository.Implementations;
using CosmosRepository.Settings;
using ElectricCalculator.DI;
using HanziCollector.DI;
using IAM.DI;
using IsolatedWorkerAutobot.Mappers;
using IsolatedWorkerAutobot.Middlewares;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = new HostBuilder()
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
        config.AddEnvironmentVariables(); // in case you're using secrets in production
    })
    .ConfigureFunctionsWorkerDefaults(worker =>
    {
        worker.UseMiddleware<AuthorizationMiddleware>();
        worker.UseNewtonsoftJson();
    })
    .ConfigureOpenApi()
    .ConfigureServices((context, services) =>
    {
        // Bind config to a strongly typed options class (recommended)
        services.Configure<CosmosDbSettings>(context.Configuration.GetSection("CosmosDb"));
        var cosmosConfig = context.Configuration.GetSection("CosmosDb").Get<CosmosDbSettings>();
        Console.WriteLine($"[DEBUG] Cosmos Account: {cosmosConfig?.Account}");
        Console.WriteLine($"[DEBUG] Cosmos Key: {(string.IsNullOrEmpty(cosmosConfig?.Key) ? "MISSING" : "LOADED")}");

        services.Configure<CosmosDbSettings>(context.Configuration.GetSection("CosmosDb"));

        // Register services
        services.AddSingleton<CosmosDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddAutoMapper(typeof(VocabularyProfile));
        services.SetupHanziDependencies();
        services.SetupElectricityPriceDependencies();
        services.SetupIAMDependencies();
        services.AddEndpointsApiExplorer();
    })
    .Build();

host.Run();

