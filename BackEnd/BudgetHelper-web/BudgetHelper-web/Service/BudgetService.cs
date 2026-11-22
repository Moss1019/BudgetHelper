using BudgetApp_lib.Model;
using BudgetApp_lib.Result;
using BudgetHelper_web.Repository;

namespace BudgetHelper_web.Service;

/// <summary>
/// Only one budget exists at a time: the app is used by a single person
/// This makes the logic easy, just work on a single record
/// </summary>

public class BudgetService
{
    private BudgetRepository _repo;
    
    public BudgetService(BudgetRepository repo)
    {
        _repo = repo;
    }
    
    public OpResult<Budget> Create(Budget entity)
    {
        return _repo.Create(entity);
    }

    public OpResult<Budget> Get()
    {
        return _repo.Get();
    }
    
    public OpResult<Budget> Update(Budget entity)
    {
        return _repo.Update(entity);
    }
    
}