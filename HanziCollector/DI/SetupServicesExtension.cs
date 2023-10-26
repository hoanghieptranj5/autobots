using HanziCollector.Abstraction;
using HanziCollector.Implementations;
using HanziCollector.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace HanziCollector.DI;

public static class SetupServicesExtension
{
    public static void SetupHanziDependencies(this IServiceCollection services)
    {
        #region Internal Interfaces

        services.AddScoped<ITextDocumentReader, TextDocumentReader>();
        services.AddScoped<IHanziDbService, HanziDbService>();
        services.AddScoped<ICrawlerService, CrawlerService>();

        #endregion

        #region Public Interfaces

        services.AddScoped<IHanziService, HanziService>();
        services.AddAutoMapper(typeof(HanziProfile));

        #endregion
    }
}
