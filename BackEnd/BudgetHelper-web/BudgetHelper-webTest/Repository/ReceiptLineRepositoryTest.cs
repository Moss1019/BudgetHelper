using BudgetApp_lib.Model;
using BudgetApp_lib.ModelContext;
using BudgetHelper_web.Repository;
using Microsoft.EntityFrameworkCore;

namespace BudgetHelper_webTest.Repository;

public class ReceiptLineRepositoryTest
{
    private ReceiptLineRepository sut = null!;
    private DbContextOptions<BudgetHelperDbContext> options = null!;

    [SetUp]
    public void Setup()
    {
        options = new DbContextOptionsBuilder<BudgetHelperDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    private BudgetHelperDbContext CreateDbContext()
    {
        return new BudgetHelperDbContext(options);
    }

    private ReceiptLineRepository CreateRepository()
    {
        var contextFactory = new InMemoryContextFactory(options);
        return new ReceiptLineRepository(contextFactory);
    }

    private class InMemoryContextFactory(DbContextOptions<BudgetHelperDbContext> options) : IDbContextFactory<BudgetHelperDbContext>
    {
        public BudgetHelperDbContext CreateDbContext() => new BudgetHelperDbContext(options);
    }

    [Test]
    public void Create_AddsReceiptLineToDatabase()
    {
        // Arrange
        var testReceiptId = Guid.NewGuid().ToString();
        using (var context = CreateDbContext())
        {
            context.Receipts.Add(new Receipt { Id = testReceiptId, Date = DateTime.UtcNow, Amount = 0 });
            context.SaveChanges();
        }
        sut = CreateRepository();
        
        var testId = Guid.NewGuid().ToString();
        var receiptLine = new ReceiptLine { Id = testId, ReceiptId = testReceiptId, Title = "test item", Amount = 150 };

        // Act
        var result = sut.Create(receiptLine);

        // Assert
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Value!.Id, Is.EqualTo(testId));

        // Verify in database
        using (var context = CreateDbContext())
        {
            var savedReceiptLine = context.ReceiptLines.Find(testId);
            Assert.That(savedReceiptLine, Is.Not.Null);
            Assert.That(savedReceiptLine!.Amount, Is.EqualTo(150));
        }
    }

    [Test]
    public void Get_ReturnsReceiptLinesForReceipt_WhenExists()
    {
        // Arrange
        var testReceiptId = Guid.NewGuid().ToString();
        var testReceiptLineId = Guid.NewGuid().ToString();
        using (var context = CreateDbContext())
        {
            context.Receipts.Add(new Receipt { Id = testReceiptId, Date = DateTime.UtcNow, Amount = 75 });
            context.ReceiptLines.Add(new ReceiptLine { Id = testReceiptLineId, ReceiptId = testReceiptId, Title = "Test item",  Amount = 75 });
            context.SaveChanges();
        }
        sut = CreateRepository();

        // Act
        var result = sut.GetForReceipt(testReceiptId);

        // Assert
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.Count, Is.GreaterThan(0));
        Assert.That(result.Value[0].ReceiptId, Is.EqualTo(testReceiptId));
    }

    [Test]
    public void Get_FailedResult_WhenNotExists()
    {
        // Arrange
        sut = CreateRepository();

        // Act
        var result = sut.Get("non-existent");

        // Assert
        Assert.That(result.IsSuccessful, Is.False);
        Assert.That(result.Value, Is.Null);
    }

    [Test]
    public void Update_UpdatesExistingReceipt()
    {
        // Arrange
        using (var context = CreateDbContext())
        {
            context.ReceiptLines.Add(new ReceiptLine { Id = "update-1", ReceiptId = "update-1", Title = "test item", Amount = 50 });
            context.SaveChanges();
        }
        sut = CreateRepository();

        using (var context = CreateDbContext())
        {
            var receiptLine = context.ReceiptLines.Find("update-1");
            Assert.That(receiptLine, Is.Not.Null);
            receiptLine.Amount = 200;

            // Act
            var result = sut.Update(receiptLine);

            // Assert
            Assert.That(result.IsSuccessful, Is.True);
        }

        // Verify in database
        using (var context = CreateDbContext())
        {
            var updatedReceipt = context.ReceiptLines.Find("update-1");
            Assert.That(updatedReceipt, Is.Not.Null);
            Assert.That(updatedReceipt!.Amount, Is.EqualTo(200));
        }
    }

    [Test]
    public void Delete_RemovesReceiptLine()
    {
        // Arrange
        using (var context = CreateDbContext())
        {
            context.ReceiptLines.Add(new ReceiptLine { Id = "delete-1", ReceiptId = "delete-1", Title = "Test item", Amount = 30 });
            context.SaveChanges();
        }
        sut = CreateRepository();

        // Act
        var result = sut.Delete("delete-1");

        // Assert
        Assert.That(result.IsSuccessful, Is.True);

        // Verify in database
        using (var context = CreateDbContext())
        {
            var deletedReceipt = context.ReceiptLines.Find("delete-1");
            Assert.That(deletedReceipt, Is.Null);
        }
    }
}