using BudgetHelper_ef;
using Microsoft.EntityFrameworkCore;

// dotnet ef migrations add <MigrationName>
// dotnet ef database update

var db = new BudgetHelperDbContextFactory().CreateDbContext(args);
db.Database.Migrate();