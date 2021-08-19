using System.IO;
using Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DataStore.Factories
{
    public class EmployeesContextFactory : IDesignTimeDbContextFactory<EmployeesContext>
    {
        public EmployeesContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory().Replace("DataStore", "Api"))
                .AddJsonFile("appsettings.json")
                .Build();

            var dataStoreConfiguration = configuration.GetSection("DataStore").Initialize<DataStoreConfiguration>();
            dataStoreConfiguration.TryValidateObject();

            var optionsBuilder = new DbContextOptionsBuilder<EmployeesContext>();
            optionsBuilder.UseNpgsql(dataStoreConfiguration.ConnectionString);
            return new EmployeesContext(optionsBuilder.Options);
        }
    }
}