namespace BudgetHelper_web.Ollama.Model;

public struct ExtractedReceiptLine
{
    public string Title;

    public double Price;

    public override string ToString()
    {
        return $"{Title} @{Price}";
    }
}

public struct ExtractedReceipt
{
    public ExtractedReceiptLine[] Lines;
}