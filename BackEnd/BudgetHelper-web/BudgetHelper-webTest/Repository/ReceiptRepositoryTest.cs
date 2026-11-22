using BudgetApp_lib.Model;
using BudgetApp_lib.ModelContext;
using BudgetHelper_web.Repository;
using Microsoft.EntityFrameworkCore;

namespace BudgetHelper_webTest.Repository;

[TestFixture]
public class ReceiptRepositoryTest
{
    private ReceiptRepository sut = null!;
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

    private ReceiptRepository CreateRepository()
    {
        var contextFactory = new InMemoryContextFactory(options);
        return new ReceiptRepository(contextFactory);
    }

    private class InMemoryContextFactory(DbContextOptions<BudgetHelperDbContext> options) : IDbContextFactory<BudgetHelperDbContext>
    {
        public BudgetHelperDbContext CreateDbContext() => new BudgetHelperDbContext(options);
    }

    [Test]
    public void GetAll_ReturnsEmptyList_WhenNoReceipts()
    {
        // Arrange
        sut = CreateRepository();

        // Act
        var result = sut.GetAll();

        // Assert
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Value, Is.Empty);
    }

    [Test]
    public void GetAll_ReturnsAllReceipts_WhenReceiptsExist()
    {
        // Arrange
        using (var context = CreateDbContext())
        {
            context.Receipts.Add(new Receipt { Id = "1", Date = DateTime.UtcNow, Amount = 100 });
            context.Receipts.Add(new Receipt { Id = "2", Date = DateTime.UtcNow, Amount = 200 });
            context.SaveChanges();
        }
        sut = CreateRepository();

        // Act
        var result = sut.GetAll();

        // Assert
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Value, Has.Count.EqualTo(2));
    }

    [Test]
    public void Create_AddsReceiptToDatabase()
    {
        // Arrange
        sut = CreateRepository();
        var testId = Guid.NewGuid().ToString();
        var receipt = new Receipt { Id = testId, Date = DateTime.UtcNow, Amount = 150 };

        // Act
        var result = sut.Create(receipt);

        // Assert
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Value!.Id, Is.EqualTo(testId));

        // Verify in database
        using (var context = CreateDbContext())
        {
            var savedReceipt = context.Receipts.Find(testId);
            Assert.That(savedReceipt, Is.Not.Null);
            Assert.That(savedReceipt!.Amount, Is.EqualTo(150));
        }
    }

    [Test]
    public void Get_ReturnsReceipt_WhenExists()
    {
        // Arrange
        using (var context = CreateDbContext())
        {
            context.Receipts.Add(new Receipt { Id = "get-1", Date = DateTime.UtcNow, Amount = 75 });
            context.SaveChanges();
        }
        sut = CreateRepository();

        // Act
        var result = sut.Get("get-1");

        // Assert
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Id, Is.EqualTo("get-1"));
        Assert.That(result.Value.Amount, Is.EqualTo(75));
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
            context.Receipts.Add(new Receipt { Id = "update-1", Date = DateTime.UtcNow, Amount = 50 });
            context.SaveChanges();
        }
        sut = CreateRepository();

        using (var context = CreateDbContext())
        {
            var receipt = context.Receipts.Find("update-1");
            Assert.That(receipt, Is.Not.Null);
            receipt.Amount = 200;

            // Act
            var result = sut.Update(receipt);

            // Assert
            Assert.That(result.IsSuccessful, Is.True);
        }

        // Verify in database
        using (var context = CreateDbContext())
        {
            var updatedReceipt = context.Receipts.Find("update-1");
            Assert.That(updatedReceipt, Is.Not.Null);
            Assert.That(updatedReceipt!.Amount, Is.EqualTo(200));
        }
    }

    [Test]
    public void Delete_RemovesReceipt()
    {
        // Arrange
        using (var context = CreateDbContext())
        {
            context.Receipts.Add(new Receipt { Id = "delete-1", Date = DateTime.UtcNow, Amount = 30 });
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
            var deletedReceipt = context.Receipts.Find("delete-1");
            Assert.That(deletedReceipt, Is.Null);
        }
    }
}
