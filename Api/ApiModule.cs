using Affecto.Mapping.AutoMapper.Autofac;
using Application;
using Autofac;
using DataStore;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace RestApiTask
{
    public class ApiModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var configurationRoot = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();
            builder.RegisterInstance(configurationRoot).As<IConfigurationRoot>();
            
            builder.RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder.ConfigureAutoMapper();
            RegisterMappers(builder);

            builder.RegisterModule(new DataStoreModule(configurationRoot));
            builder.RegisterModule<ApplicationModule>();
            
            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
        }

        private static void RegisterMappers(ContainerBuilder builder)
        {
            
        }
    }
}