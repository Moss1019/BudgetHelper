using BudgetApp_lib.Model;
using BudgetApp_lib.Result;
using BudgetHelper_web.Repository;

namespace BudgetHelper_web.Service;

public abstract class Service<T> where T: Item
{
    protected readonly Repository<T> _repo;

    public Service(Repository<T> repo)
    {
        _repo = repo;
    }
    
    public virtual OpResult<T> Create(T entity)
    {
        if (String.IsNullOrEmpty(entity.Id))
        {
            entity.Id = Guid.NewGuid().ToString();
        }
        return _repo.Create(entity);
    }
    
    public virtual OpResult<T?> Get(string id)
    {
        return _repo.Get(id);
    }
    
    public virtual OpResult<List<T>> GetMany(List<string> ids)
    {
        if (ids.Count == 0)
        {
            return OpResult<List<T>>.Success(new List<T>());
        }

        return _repo.GetMany(ids);
    }
    
    public virtual OpResult<T?> Update(T item)
    {
        return _repo.Update(item);
    }
    
    public virtual OpResult<T?> Delete(string id)
    {
        return _repo.Delete(id);
    }
    
}