using BudgetApp_lib.Model;
using BudgetApp_lib.ModelContext;
using BudgetApp_lib.Result;
using Microsoft.EntityFrameworkCore;

namespace BudgetHelper_web.Repository;

public class ReceiptLineRepository : Repository<ReceiptLine>
{
    public ReceiptLineRepository(IDbContextFactory<BudgetHelperDbContext> contextFactory) : base(contextFactory)
    {
    }

    public OpResult<List<ReceiptLine>> GetForReceipt(string receiptId)
    {
        var items = Context.Set<ReceiptLine>().Where((i) => i.ReceiptId == receiptId).ToList();

        if (items.Count == 0)
        {
            return OpResult<List<ReceiptLine>>.Failed($"Could not find any items with the provided IDs");
        }
        
        return OpResult<List<ReceiptLine>>.Success(items);
    }
}