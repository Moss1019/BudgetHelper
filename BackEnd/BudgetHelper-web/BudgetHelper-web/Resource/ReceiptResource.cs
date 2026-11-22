using BudgetApp_lib.Model;
using BudgetHelper_web.Service;

namespace BudgetHelper_web.Resource;

public static class ReceiptResource
{
    public static void MapReceiptEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/receipts");

        group.MapGet("/{id}", (string id, ReceiptService service) =>
        {
            var result = service.Get(id);
            if (result.IsSuccessful)
            {
                return Results.Ok(result.Value);
            }
            return Results.NotFound(result.Message);
        });

        group.MapGet("/", (ReceiptService service) =>
        {
            var result = service.GetAll();
            if (result.IsSuccessful)
            {
                return Results.Ok(result.Value);
            }
            return Results.Ok(new List<Receipt>());
        });

        group.MapGet("/lines/{id}", (string id, ReceiptLineService service) =>
        {
            var result = service.GetForReceipt(id);
            if (result.IsSuccessful)
            {
                return Results.Ok(result.Value);
            }
            return Results.Ok(new List<ReceiptLine>());
        });

        group.MapPost("/", (Receipt receipt, ReceiptService service) =>
        {
            var result = service.Create(receipt);
            if (result.IsSuccessful)
            {
                return Results.Created($"/receipts/{result.Value.Id}", result.Value);
            }
            return Results.BadRequest(result.Message);
        });

        group.MapPut("/{id}", (string id, Receipt receipt, ReceiptService service) =>
        {
            if (receipt.Id != id)
            {
                return Results.BadRequest("ID in path must match ID in body");
            }

            var result = service.Update(receipt);
            if (result.IsSuccessful)
            {
                return Results.Ok(result.Value);
            }
            return result.Value == null ? Results.NotFound() : Results.BadRequest(result.Message);
        });

        group.MapDelete("/{id}", (string id, ReceiptService service) =>
        {
            var result = service.Delete(id);
            if (result.IsSuccessful)
            {
                return Results.NoContent();
            }
            return result.Value == null ? Results.NotFound() : Results.BadRequest(result.Message);
        });
    }
}
