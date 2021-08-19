using System.Threading.Tasks;
using DataStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace DataStore
{
    public class EmployeesContext : DbContext, IEmployeesContext
    {
#if DEBUG
        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole(options =>
            {
                options.FormatterName = ConsoleFormatterNames.Systemd;
                options.LogToStandardErrorThreshold = LogLevel.Information;
            });
        });
#endif
        private IDbContextTransaction? transaction;
        public DbSet<Employee> Employees { get; set; } = default!;

        public EmployeesContext(DbContextOptions<EmployeesContext> options)
            : base(options)
        {
        }
        
        public void BeginTransaction()
        {
            transaction = Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                SaveChanges();
                transaction?.Commit();
            }
            finally
            {
                transaction?.Dispose();
            }
        }

        public void Rollback()
        {
            if (transaction != null)
            {
                try
                {
                    transaction.Rollback();
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }

        void IEmployeesContext.SaveChanges()
        {
            base.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }

        public async Task<bool> EmployeeWithCeoRoleExist()
        {
            return await Employees.AnyAsync(x => x.Role == Role.Ceo);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
            
            if (loggerFactory is not null)
            {
                optionsBuilder.UseLoggerFactory(loggerFactory);
            }
#endif
        }
    }
}