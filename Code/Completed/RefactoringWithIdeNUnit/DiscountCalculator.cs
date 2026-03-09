namespace RefactoringWithIdeNUnit;

public class DiscountCalculator
{
    public decimal CalculateTotal(decimal unitPrice, int quantity,
        decimal discountThreshold = 100m, decimal discountAmount = 10m,
        int decimalPlaces = 2)
    {
        decimal total = unitPrice * quantity;
        if (total > discountThreshold)
            total -= discountAmount;
        return Math.Round(total, decimalPlaces);
    }
}
