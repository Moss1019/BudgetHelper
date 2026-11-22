using BudgetApp_lib.Model;
using BudgetHelper_web.Service;

namespace BudgetHelper_web.Resource;

public static class BudgetResource
{
    public static void MapBudgetEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/budgets");

        group.MapGet("/", (BudgetService service) =>
        {
            var getRes = service.Get();
            if (!getRes.IsSuccessful)
            {
                return Results.BadRequest(getRes.Message);
            }
            
            return Results.Ok(getRes.Value);
        });

        group.MapPost("/",
            (Budget budget, BudgetService service) =>
            {
                var extractRes = service.Create(budget);
                if (!extractRes.IsSuccessful)
                {
                    Console.Out.WriteLine(extractRes.Message);
                    return Results.BadRequest(extractRes.Message);
                }

                return Results.Ok(extractRes.Value);
            }
        );
        
        group.MapPut("/",
            (Budget budget, BudgetService service) =>
            {
                var extractRes = service.Update(budget);
                if (!extractRes.IsSuccessful)
                {
                    return Results.BadRequest(extractRes.Message);
                }

                return Results.Ok(extractRes.Value);
            }
        );
    }
}