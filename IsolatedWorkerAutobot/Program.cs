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
        config.AddEnvironmentVariables(); // Load env vars from Azure Function config or local env
    })
    .ConfigureFunctionsWorkerDefaults(worker =>
    {
        worker.UseMiddleware<AuthorizationMiddleware>();
        worker.UseNewtonsoftJson();
    })
    .ConfigureOpenApi()
    .ConfigureServices((context, services) =>
    {
        // 💡 Get connection string from environment variable
        var cosmosConnection = Environment.GetEnvironmentVariable("COSMOSDB_CONNECTIONSTRING");
        var databaseName = Environment.GetEnvironmentVariable("COSMOSDB_DATABASE");

        if (string.IsNullOrEmpty(cosmosConnection))
            throw new InvalidOperationException("COSMOSDB_CONNECTIONSTRING is not set in environment variables.");

        if (string.IsNullOrEmpty(databaseName))
            throw new InvalidOperationException("COSMOSDB_DATABASE is not set in environment variables.");

        services.Configure<CosmosDbSettings>(options =>
        {
            options.ConnectionString = cosmosConnection;
            options.DatabaseName = databaseName;
        });

        // 🔧 Register services
        services.AddSingleton<CosmosDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddAutoMapper(typeof(VocabularyProfile));
        services.SetupHanziDependencies();
        services.SetupElectricityPriceDependencies();
        services.SetupIAMDependencies();
        services.AddEndpointsApiExplorer();

        // 👀 Optional: debug output
        Console.WriteLine($"[DEBUG] CosmosDbSettings loaded: {(string.IsNullOrEmpty(cosmosConnection) ? "MISSING" : "OK")}");
    })
    .Build();

host.Run();
