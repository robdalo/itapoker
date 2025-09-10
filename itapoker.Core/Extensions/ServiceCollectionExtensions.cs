using itapoker.Core.Interfaces;
using itapoker.Core.Repositories;
using itapoker.Core.Repositories.Interfaces;
using itapoker.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace itapoker.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddItaPoker(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => {
            cfg.AddProfile<DomainToSDK>();
            cfg.AddProfile<SDKToDomain>();
            cfg.LicenseKey = "<License Key Here>";
        });

        services.AddSingleton<IGameRepo, GameRepo>();
        services.AddSingleton<IHighScoreRepo, HighScoreRepo>();

        services.AddSingleton<IDecisionService, DecisionService>();
        services.AddSingleton<IDealerService, DealerService>();

        services.AddSingleton<IGameEngine, GameEngine>();

        return services;
    }
}