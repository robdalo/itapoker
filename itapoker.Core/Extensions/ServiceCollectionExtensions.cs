using itapoker.Core.Domain.Models;
using itapoker.Core.Interfaces;
using itapoker.Core.Repositories;
using itapoker.Core.Repositories.Interfaces;
using itapoker.Core.Services;
using itapoker.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace itapoker.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddItaPoker(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => {
            cfg.AddProfile<DomainToSDK>();
            cfg.AddProfile<SDKToDomain>();
            cfg.LicenseKey = "";
        });

        services.AddOptions<GameSettings>();

        services.AddSingleton<IGameRepo, GameRepo>();
        services.AddSingleton<IHighScoreRepo, HighScoreRepo>();

        services.AddSingleton<IBetService, BetService>();
        services.AddSingleton<ICardService, CardService>();
        services.AddSingleton<IDecisionService, DecisionService>();
        services.AddSingleton<ISecurityService, SecurityService>();
        services.AddSingleton<IValidationService, ValidationService>();

        services.AddSingleton<IGameEngine, GameEngine>();

        return services;
    }
}