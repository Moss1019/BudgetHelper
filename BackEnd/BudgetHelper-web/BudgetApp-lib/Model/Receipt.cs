namespace BudgetApp_lib.Model;

public class Receipt : Item
{
    public required DateTime Date { get; set; }
    
    public required decimal Amount { get; set; }
}