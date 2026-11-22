using BudgetApp_lib.Model;
using BudgetApp_lib.Result;
using BudgetHelper_web.Repository;

namespace BudgetHelper_web.Service;

public class ReceiptLineService : Service<ReceiptLine>
{
    public ReceiptLineService(ReceiptLineRepository repo): base(repo)
    {
        
    }

    public OpResult<List<ReceiptLine>> GetForReceipt(string receiptId)
    {
        if (String.IsNullOrEmpty(receiptId))
        {
            return OpResult<List<ReceiptLine>>.Failed("receiptId is empty");
        }
        return (_repo as ReceiptLineRepository).GetForReceipt(receiptId);
    }
}