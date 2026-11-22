namespace BudgetHelper_ef;

using BudgetApp_lib.ModelContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

class BudgetHelperDbContextFactory : IDesignTimeDbContextFactory<BudgetHelperDbContext>
{
    public BudgetHelperDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config.GetConnectionString("Postgres") ?? throw new InvalidOperationException("Postgres connection string missing");

        var options = new DbContextOptionsBuilder<BudgetHelperDbContext>()
            .UseNpgsql(connectionString, o => o.MigrationsAssembly("BudgetHelper-ef"))
            .Options;
        
        return new BudgetHelperDbContext(options);
    }
}