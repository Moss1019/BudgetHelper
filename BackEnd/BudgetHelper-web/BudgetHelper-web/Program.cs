using BudgetApp_lib.ModelContext;
using BudgetHelper_web.Repository;
using BudgetHelper_web.Resource;
using BudgetHelper_web.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<BudgetHelperDbContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

builder.Services.AddSingleton<ReceiptRepository>();
builder.Services.AddSingleton<ReceiptService>();

builder.Services.AddSingleton<BudgetRepository>();
builder.Services.AddSingleton<BudgetService>();

builder.Services.AddSingleton<ReceiptLineRepository>();
builder.Services.AddSingleton<ReceiptLineService>();

builder.Services.AddSingleton<ReceiptDetectorService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapReceiptEndpoints();
app.MapReceiptLineEndpoints();
app.MapReceiptDetectorEndpoints();
app.MapBudgetEndpoints();

app.Run();
