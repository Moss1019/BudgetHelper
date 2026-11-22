using BudgetApp_lib.Model;
using BudgetHelper_web.Service;

namespace BudgetHelper_web.Resource;

public static class ReceiptLineResource
{
    public static void MapReceiptLineEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/receipt-lines");

        group.MapGet("/{id}", (string id, ReceiptLineService service) =>
        {
            var result = service.Get(id);
            if (result.IsSuccessful)
            {
                return Results.Ok(result.Value);
            }
            return Results.NotFound(result.Message);
        });

        group.MapPost("/", (ReceiptLine receipt, ReceiptLineService service) =>
        {
            var result = service.Create(receipt);
            if (result.IsSuccessful)
            {
                return Results.Created($"/receipt-line/{result.Value.Id}", result.Value);
            }
            return Results.BadRequest(result.Message);
        });

        group.MapPut("/{id}", (string id, ReceiptLine receipt, ReceiptLineService service) =>
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

        group.MapDelete("/{id}", (string id, ReceiptLineService service) =>
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