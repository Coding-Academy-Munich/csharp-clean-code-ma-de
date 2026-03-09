namespace RefactoringWithAiNUnit;

public record ShopItem(string Name, double Price, int Quantity, string Category);

public class ShoppingCart
{
    private readonly List<ShopItem> _items = new();
    private static readonly Dictionary<string, double> TaxRates = new()
    {
        ["food"] = 0.07,
        ["electronics"] = 0.19
    };
    private const double DefaultTaxRate = 0.19;

    public void AddItem(ShopItem item) => _items.Add(item);

    public double CalculateTotal()
    {
        double subtotalWithTax = _items.Sum(item => CalculateItemTotal(item));
        double discountRate = DetermineDiscountRate(subtotalWithTax);
        return subtotalWithTax * (1 - discountRate);
    }

    private static double CalculateItemTotal(ShopItem item)
    {
        double taxRate = TaxRates.GetValueOrDefault(item.Category, DefaultTaxRate);
        return item.Price * item.Quantity * (1 + taxRate);
    }

    private static double DetermineDiscountRate(double total)
        => total > 100 ? 0.10 : total > 50 ? 0.05 : 0;
}
