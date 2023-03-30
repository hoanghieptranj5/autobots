using HanziCollector.Abstraction;
using HanziCollector.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace HanziCollector.DI;

public static class SetupServicesExtension
{
    public static void SetupHanziDependencies(this IServiceCollection services)
    {
        services.AddScoped<ITextDocumentReader, TextDocumentReader>();
        services.AddScoped<IHanziDbService, HanziDbService>();
        services.AddScoped<IHanziService, HanziService>();
        services.AddScoped<ICrawlerService, CrawlerService>();
    }
}