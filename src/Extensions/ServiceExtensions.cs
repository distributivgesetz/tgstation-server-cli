namespace Tgstation.Server.CommandLineInterface.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Middlewares;
using Commands;

public static class ServiceExtensions
{
    public static IServiceCollection UseCommand(this IServiceCollection container, Type commandType) =>
        container.AddTransient(commandType, services =>
        {
            var inst = ActivatorUtilities.CreateInstance(services, commandType);

            if (!commandType.IsSubclassOf(typeof(BaseCommand)))
            {
                return inst;
            }

            var baseCommand = (BaseCommand)inst;
            baseCommand.UseMiddlewares(services.GetRequiredService<IMiddlewarePipeline>());

            return inst;
        });
}