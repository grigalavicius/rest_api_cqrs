using System.Collections.Generic;
using Application.Commands.CreateEmployee;
using Application.Commands.DeleteEmployee;
using Application.Commands.UpdateEmployee;
using Application.Commands.UpdateEmployeesSalary;
using Application.Mappers;
using Application.Queries.GetAllEmployees;
using Application.Queries.GetEmployeeById;
using Application.Queries.GetEmployeesByBossId;
using Application.Queries.GetEmployeesByNameAndBirthdateInterval;
using Application.Queries.GetEmployeesStatisticsByRole;
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
            builder.RegisterType<GetEmployeesByBossIdQueryHandler>().AsImplementedInterfaces();
            builder.RegisterType<GetEmployeeByIdQueryHandler>().AsImplementedInterfaces();
            builder.RegisterType<GetEmployeesByNameAndBirthdateIntervalQueryHandler>().AsImplementedInterfaces();
            builder.RegisterType<GetStatisticsByRoleQueryHandler>().AsImplementedInterfaces();
            builder.RegisterType<CreateEmployeeCommandHandler>().AsImplementedInterfaces();
            builder.RegisterType<UpdateEmployeeCommandHandler>().AsImplementedInterfaces();
            builder.RegisterType<UpdateEmployeesSalaryCommandHandler>().AsImplementedInterfaces();
            builder.RegisterType<DeleteEmployeeCommandHandler>().AsImplementedInterfaces();
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