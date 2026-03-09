namespace RefactoringWithAiNUnitSk;

[TestFixture]
public class ShopTest
{
    private Shop _shop = null!;

    [SetUp]
    public void SetUp()
    {
        _shop = new Shop();
    }

    [Test]
    public void CalcTotal_EmptyCart_ReturnsZero()
    {
        Assert.That(_shop.CalcTotal(), Is.EqualTo(0));
    }

    [Test]
    public void CalcTotal_SingleFoodItem_Applies7PercentTax()
    {
        _shop.items.Add(("Bread", 10.0, 1, "food"));

        Assert.That(_shop.CalcTotal(), Is.EqualTo(10.70).Within(0.001));
    }

    [Test]
    public void CalcTotal_SingleElectronicsItem_Applies19PercentTax()
    {
        _shop.items.Add(("Cable", 10.0, 1, "electronics"));

        Assert.That(_shop.CalcTotal(), Is.EqualTo(11.90).Within(0.001));
    }

    [Test]
    public void CalcTotal_UnknownCategory_AppliesDefaultTax()
    {
        _shop.items.Add(("Book", 10.0, 1, "books"));

        Assert.That(_shop.CalcTotal(), Is.EqualTo(11.90).Within(0.001));
    }

    [Test]
    public void CalcTotal_MultipleQuantity_MultipliesCorrectly()
    {
        _shop.items.Add(("Apple", 5.0, 3, "food"));

        Assert.That(_shop.CalcTotal(), Is.EqualTo(16.05).Within(0.001));
    }

    [Test]
    public void CalcTotal_Over100_Applies10PercentDiscount()
    {
        _shop.items.Add(("Laptop", 100.0, 1, "electronics"));

        // 100 * 1.19 = 119.0, discount 10% => 107.10
        Assert.That(_shop.CalcTotal(), Is.EqualTo(107.10).Within(0.001));
    }

    [Test]
    public void CalcTotal_Over50UpTo100_Applies5PercentDiscount()
    {
        _shop.items.Add(("Headphones", 50.0, 1, "electronics"));

        // 50 * 1.19 = 59.50, discount 5% => 56.525
        Assert.That(_shop.CalcTotal(), Is.EqualTo(56.525).Within(0.001));
    }

    [Test]
    public void CalcTotal_Under50_NoDiscount()
    {
        _shop.items.Add(("Snack", 5.0, 1, "food"));

        // 5 * 1.07 = 5.35, no discount
        Assert.That(_shop.CalcTotal(), Is.EqualTo(5.35).Within(0.001));
    }

    [Test]
    public void CalcTotal_MixedItems_CalculatesCorrectly()
    {
        _shop.items.Add(("Bread", 10.0, 2, "food"));
        _shop.items.Add(("Cable", 20.0, 1, "electronics"));

        // Food: 10*2*1.07 = 21.40, Electronics: 20*1*1.19 = 23.80
        // Total: 45.20, no discount
        Assert.That(_shop.CalcTotal(), Is.EqualTo(45.20).Within(0.001));
    }
}
