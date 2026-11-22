using BudgetApp_lib.Model;
using BudgetApp_lib.Result;
using BudgetHelper_web.Repository;

namespace BudgetHelper_web.Service;

public class ReceiptService: Service<Receipt>
{
    public ReceiptService(ReceiptRepository repo): base(repo)
    {
    }

    public override OpResult<Receipt> Create(Receipt entity)
    {
        if (entity.Date == default)
        {
            return OpResult<Receipt>.Failed("Date is required");
        }
        return base.Create(entity);
    }

    public OpResult<List<Receipt>> GetAll()
    {
        return (_repo as ReceiptRepository).GetAll();
    }
}