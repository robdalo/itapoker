using itapoker.Core.Interfaces;
using itapoker.Core.Repositories;
using itapoker.Core.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace itapoker.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddItaPoker(this IServiceCollection services)
    {
        services.AddSingleton<IGameRepo, GameRepo>();
        services.AddSingleton<IGameEngine, GameEngine>();

        return services;
    }
}