using BudgetApp_lib.Model;
using BudgetHelper_web.Ollama;
using BudgetHelper_web.Ollama.Model;
using BudgetHelper_web.Service;

namespace BudgetHelper_web.Resource;

public static class ReceiptDetectorResource
{
    public static void MapReceiptDetectorEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/receipt-extraction");

        group.MapPost("/",
            async (ReceiptExtractionRequest req, ReceiptDetectorService service) =>
            {
                var extractRes = await service.Extract(req);
                if (!extractRes.IsSuccessful)
                {
                    return Results.BadRequest(extractRes.Message);
                }

                return Results.Ok(extractRes.Value);
            });
    }
}