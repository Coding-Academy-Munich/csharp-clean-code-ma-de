namespace RefactoringWithIdeNUnit;

public class ShippingCalculator
{
    public decimal CalculateShippingCost(
        decimal orderTotal, decimal weightKg,
        decimal freeShippingThreshold = 100m, decimal ratePerKg = 1.50m)
    {
        if (orderTotal >= freeShippingThreshold)
            return 0m;

        decimal baseCost = 4.99m;
        decimal weightSurcharge = weightKg > 5 ? (weightKg - 5) * ratePerKg : 0m;
        return baseCost + weightSurcharge;
    }
}
