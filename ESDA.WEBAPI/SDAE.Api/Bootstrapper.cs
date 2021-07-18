using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechRAQ.Common.LoggingService.Serilog.Implementation.Modules;

namespace SDAE.Api
{
    public static class Bootstrapper
    {
        /// <summary>
        ///bootstrapper
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IContainer IntegrateContainer(IServiceCollection services, IConfiguration configuration)
        {
            var builder = new ContainerBuilder();
            RegisterTypes(builder, configuration);
            RegisterModules(builder);


            builder.Populate(services);
            return builder.Build();
        }

        private static void RegisterModules(ContainerBuilder builder)
        {
            builder.RegisterModule<LoggingModule>();
        }

        private static void RegisterTypes(ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterInstance(configuration);
        }
    }
}
