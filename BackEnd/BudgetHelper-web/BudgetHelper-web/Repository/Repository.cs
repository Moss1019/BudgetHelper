using System.Data.Common;
using BudgetApp_lib.Model;
using BudgetApp_lib.ModelContext;
using BudgetApp_lib.Result;
using Microsoft.EntityFrameworkCore;

namespace BudgetHelper_web.Repository;

public abstract class Repository<T> where T : Item
{
    private readonly BudgetHelperDbContext _context;
    
    protected BudgetHelperDbContext Context => _context;

    public Repository(IDbContextFactory<BudgetHelperDbContext> contextFactory)
    {
        _context = contextFactory.CreateDbContext();
    }

    ~Repository()
    {
        _context.Dispose();
    }

    public virtual OpResult<T> Create(T entity)
    {
        if (string.IsNullOrEmpty(entity.Id))
        {
            return OpResult<T>.Failed("Id field missing");
        }

        _context.Set<T>().Add(entity);
        
        try
        {
            _context.SaveChanges();
            return OpResult<T>.Success(entity);
        }
        catch (DbUpdateException ex)
        {
            return OpResult<T>.Failed($"Error saving changes: {ex.Message}");
        }
    }

    public virtual OpResult<T?> Get(string id)
    {
        var item = _context.Set<T>().FirstOrDefault(x => x.Id == id);
        
        if (item == null)
        {
            return OpResult<T?>.Failed($"Could not find item {id}");
        }
        
        return OpResult<T?>.Success(item);
    }

    public virtual OpResult<List<T>> GetMany(List<string> ids)
    {
        var items = _context.Set<T>().Where(i => ids.Contains(i.Id)).ToList();

        if (items.Count == 0)
        {
            return OpResult<List<T>>.Failed($"Could not find any items with the provided IDs");
        }
        
        return OpResult<List<T>>.Success(items);
    }

    public virtual OpResult<T?> Update(T item)
    {
        if (string.IsNullOrEmpty(item.Id))
        {
            return OpResult<T?>.Failed("Id field missing");
        }

        var existing = _context.Set<T>().Find(item.Id);
        
        if (existing == null)
        {
            return OpResult<T?>.Failed($"Item with ID {item.Id} not found");
        }

        _context.Entry(existing).CurrentValues.SetValues(item);
        
        try
        {
            _context.SaveChanges();
            return OpResult<T?>.Success(existing);
        }
        catch (DbUpdateException ex)
        {
            return OpResult<T?>.Failed($"Error updating item: {ex.Message}");
        }
    }

    public virtual OpResult<T?> Delete(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return OpResult<T?>.Failed("ID cannot be null or empty");
        }

        var item = _context.Set<T>().Find(id);
        
        if (item == null)
        {
            return OpResult<T?>.Failed($"Item with ID {id} not found");
        }

        _context.Set<T>().Remove(item);
        
        try
        {
            _context.SaveChanges();
            return OpResult<T?>.Success(item);
        }
        catch (DbUpdateException ex)
        {
            return OpResult<T?>.Failed($"Error deleting item: {ex.Message}");
        }
    }
}