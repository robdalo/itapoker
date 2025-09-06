using itapoker.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace itapoker.Core.Tests.Integration;

public class TestBase
{
    protected IServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddItaPoker();

        return services.BuildServiceProvider();
    }
}