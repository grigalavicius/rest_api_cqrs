using System.Collections.Generic;
using Application.Mappers;
using Application.Queries.GetAllEmployees;
using Application.Queries.GetEmployeeById;
using Application.Queries.GetEmployeesByNameAndBirthdateInterval;
using Application.Queries.GetStatisticsByRole;
using Application.Validation.ModelValidators;
using Autofac;
using AutoMapper;

namespace Application
{
    public class ApplicationModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            RegisterRequestHandlers(builder);
            RegisterMappers(builder);
        }

        private void RegisterRequestHandlers(ContainerBuilder builder)
        {
            builder.RegisterType<GetAllEmployeesQueryHandler>().AsImplementedInterfaces();
            builder.RegisterType<GetEmployeeByIdQueryHandler>().AsImplementedInterfaces();
            builder.RegisterType<GetEmployeesByNameAndBirthdateIntervalQueryHandler>().AsImplementedInterfaces();
            builder.RegisterType<GetStatisticsByRoleQueryHandler>().AsImplementedInterfaces();
        }
        
        private static void RegisterMappers(ContainerBuilder builder)
        {
            builder.RegisterType<EmployeesProfile>().As<Profile>();
            builder.RegisterType<EmployeesStatisticsByRoleProfile>().As<Profile>();
            builder.Register(c => new MapperConfiguration(cfg =>
            {
                foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            }))
                .AsSelf()
                .SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }
    }
}