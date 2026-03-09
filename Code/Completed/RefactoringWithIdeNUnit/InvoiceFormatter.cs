namespace RefactoringWithIdeNUnit;

public class InvoiceFormatter
{
    public string FormatInvoiceLine(string description, decimal unitPrice, int quantity)
    {
        decimal lineTotal = unitPrice * quantity;
        return $"{description,-30} {quantity,5} x {unitPrice,10:C} = {lineTotal,12:C}";
    }

    public string FormatTotal(List<(string Description, decimal UnitPrice, int Quantity)> items)
    {
        decimal total = items.Sum(item => item.UnitPrice * item.Quantity);
        decimal tax = total * 0.19m;
        return $"Subtotal: {total:C}\nTax (19%): {tax:C}\nTotal: {total + tax:C}";
    }
}
