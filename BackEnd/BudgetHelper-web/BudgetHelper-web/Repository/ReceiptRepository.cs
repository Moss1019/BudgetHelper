using BudgetApp_lib.Model;
using BudgetApp_lib.ModelContext;
using BudgetApp_lib.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetHelper_web.Repository;

public class ReceiptRepository : Repository<Receipt>
{
    public ReceiptRepository(IDbContextFactory<BudgetHelperDbContext> contextFactory) : base(contextFactory)
    {
    }

    public OpResult<List<Receipt>> GetAll()
    {
        return OpResult<List<Receipt>>.Success(Context.Set<Receipt>().ToList());
    }
}