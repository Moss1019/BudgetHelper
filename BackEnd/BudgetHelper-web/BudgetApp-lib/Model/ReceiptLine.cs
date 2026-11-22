namespace BudgetApp_lib.Model;

public class ReceiptLine : Item
{
    public required string ReceiptId { get; set; }
    
    public required string Title { get; set; }
    
    public decimal Amount { get; set; }
}