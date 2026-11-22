using BudgetApp_lib.Model;
using BudgetApp_lib.ModelContext;
using BudgetApp_lib.Result;
using Microsoft.EntityFrameworkCore;

namespace BudgetHelper_web.Repository;

public class BudgetRepository
{
    private readonly BudgetHelperDbContext _context;
    
    public BudgetRepository(IDbContextFactory<BudgetHelperDbContext> contextFactory)
    {
        _context = contextFactory.CreateDbContext();
    }
    
    ~BudgetRepository()
    {
        _context.Dispose();
    }
    
    public OpResult<Budget> Create(Budget entity)
    {
        var existingBudget = _context.Budgets.FirstOrDefault();
        
        if (existingBudget != null)
        {
            return Update(entity);
        }
        
        _context.Budgets.Add(entity);
        
        try
        {
            _context.SaveChanges();
            return OpResult<Budget>.Success(entity);
        }
        catch (DbUpdateException ex)
        {
            return OpResult<Budget>.Failed($"Error saving changes: {ex.Message}");
        }
    }

    public OpResult<Budget> Get()
    {
        var existingBudget = _context.Budgets.FirstOrDefault();
        
        if (existingBudget != null)
        {
            return OpResult<Budget>.Success(existingBudget);
        }
        
        return OpResult<Budget>.Failed("No budget set");
    }

    public OpResult<Budget> Update(Budget entity)
    {
        var existingBudget = _context.Budgets.FirstOrDefault();
        
        if (existingBudget == null)
        {
            return OpResult<Budget>.Failed("No budget set");
        }
        
        existingBudget.MonthlyLimit = entity.MonthlyLimit;
        
        try
        {
            _context.SaveChanges();
            return OpResult<Budget>.Success(existingBudget);
        }
        catch (DbUpdateException ex)
        {
            return OpResult<Budget>.Failed($"Error updating budget: {ex.Message}");
        }
    }
}
