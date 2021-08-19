using Autofac;
using Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataStore
{
    public class DataStoreModule: Module
    {
        private readonly IConfiguration _configuration;

        public DataStoreModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var dataStoreConfiguration = _configuration.GetSection("DataStore").Initialize<DataStoreConfiguration>();
            dataStoreConfiguration.TryValidateObject();
            builder.Register(_ => new DbContextOptionsBuilder<EmployeesContext>()
                .UseNpgsql(dataStoreConfiguration.ConnectionString)
                .Options)
                .As<DbContextOptions<EmployeesContext>>()
                .IfNotRegistered(typeof(DbContextOptions<EmployeesContext>));

            builder.RegisterType<EmployeesContext>().As<IEmployeesContext>();
        }
    }
}