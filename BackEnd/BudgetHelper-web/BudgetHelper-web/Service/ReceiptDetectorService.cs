using BudgetApp_lib.Model;
using BudgetApp_lib.Result;
using BudgetHelper_web.Ollama;
using BudgetHelper_web.Ollama.Model;

namespace BudgetHelper_web.Service;

public class ReceiptDetectorService
{
    private ReceiptService _receiptService;
    
    private ReceiptLineService _receiptLineService;

    public ReceiptDetectorService(ReceiptService receiptService, ReceiptLineService receiptLineService)
    {
        _receiptService = receiptService;
        _receiptLineService = receiptLineService;
    }

    public async Task<OpResult<Receipt>> Extract(ReceiptExtractionRequest req)
    {
        var client = new OllamaClient();
        var extractReceiptRes = await client.ExtractReceipt(req.Base64Image);
        if (!extractReceiptRes.IsSuccessful)
        {
            return OpResult<Receipt>.Failed(extractReceiptRes.Message);
        }

        var newReceiptId = Guid.NewGuid().ToString();
        var receiptTotal = 0M;
        var convertedLines = new List<ReceiptLine>();
        foreach (var extractedLine in extractReceiptRes.Value.Lines)
        {
            var price = (decimal)extractedLine.Price;
            receiptTotal += price;
            convertedLines.Add(new ReceiptLine
            {
                Title = extractedLine.Title,
                Amount = price,
                ReceiptId = newReceiptId,
                Id = ""
            });
        }

        var createReceiptRes = _receiptService.Create(new Receipt
        {
            Amount = receiptTotal,
            Date = DateTime.Now,
            Id = newReceiptId
        });
        if (!createReceiptRes.IsSuccessful)
        {
            return OpResult<Receipt>.Failed(createReceiptRes.Message);
        }

        foreach (var line in convertedLines)
        {
            var createReceiptLineRes = _receiptLineService.Create(line);
            if (!createReceiptLineRes.IsSuccessful)
            {
                return OpResult<Receipt>.Failed(createReceiptLineRes.Message);
            }
        }

        return OpResult<Receipt>.Success(createReceiptRes.Value);
    }
}