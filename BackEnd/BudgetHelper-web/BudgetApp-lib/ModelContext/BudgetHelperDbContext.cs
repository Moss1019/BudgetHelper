using System.Runtime.Intrinsics.X86;
using BudgetApp_lib.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BudgetApp_lib.ModelContext;

public class UtcDateTimeConverter() : ValueConverter<DateTime, DateTime>(
    v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc),
    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
); 

public class BudgetHelperDbContext(DbContextOptions<BudgetHelperDbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<Receipt> Receipts => Set<Receipt>();
    public DbSet<ReceiptLine> ReceiptLines => Set<ReceiptLine>();
    public DbSet<Budget> Budgets => Set<Budget>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveConversion<UtcDateTimeConverter>();
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Receipt>(e =>
        {
            e.Property(o => o.Id).HasMaxLength(36);
            e.HasIndex(o => o.Date);
        });
        mb.Entity<ReceiptLine>(e =>
        {
            e.Property(o => o.Id).HasMaxLength(36);
            e.Property(o => o.ReceiptId).HasMaxLength(36);
            e.HasOne<Receipt>()
                .WithMany()
                .HasForeignKey(o => o.ReceiptId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}