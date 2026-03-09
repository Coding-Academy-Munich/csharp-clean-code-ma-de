namespace RefactoringWithAiNUnit;

[TestFixture]
public class ShoppingCartTest
{
    private ShoppingCart _cart = null!;

    [SetUp]
    public void SetUp()
    {
        _cart = new ShoppingCart();
    }

    [Test]
    public void CalculateTotal_EmptyCart_ReturnsZero()
    {
        Assert.That(_cart.CalculateTotal(), Is.EqualTo(0));
    }

    [Test]
    public void CalculateTotal_SingleFoodItem_Applies7PercentTax()
    {
        _cart.AddItem(new ShopItem("Bread", 10.0, 1, "food"));

        Assert.That(_cart.CalculateTotal(), Is.EqualTo(10.70).Within(0.001));
    }

    [Test]
    public void CalculateTotal_SingleElectronicsItem_Applies19PercentTax()
    {
        _cart.AddItem(new ShopItem("Cable", 10.0, 1, "electronics"));

        Assert.That(_cart.CalculateTotal(), Is.EqualTo(11.90).Within(0.001));
    }

    [Test]
    public void CalculateTotal_UnknownCategory_AppliesDefaultTax()
    {
        _cart.AddItem(new ShopItem("Book", 10.0, 1, "books"));

        Assert.That(_cart.CalculateTotal(), Is.EqualTo(11.90).Within(0.001));
    }

    [Test]
    public void CalculateTotal_MultipleQuantity_MultipliesCorrectly()
    {
        _cart.AddItem(new ShopItem("Apple", 5.0, 3, "food"));

        Assert.That(_cart.CalculateTotal(), Is.EqualTo(16.05).Within(0.001));
    }

    [Test]
    public void CalculateTotal_Over100_Applies10PercentDiscount()
    {
        _cart.AddItem(new ShopItem("Laptop", 100.0, 1, "electronics"));

        // 100 * 1.19 = 119.0, discount 10% => 107.10
        Assert.That(_cart.CalculateTotal(), Is.EqualTo(107.10).Within(0.001));
    }

    [Test]
    public void CalculateTotal_Over50UpTo100_Applies5PercentDiscount()
    {
        _cart.AddItem(new ShopItem("Headphones", 50.0, 1, "electronics"));

        // 50 * 1.19 = 59.50, discount 5% => 56.525
        Assert.That(_cart.CalculateTotal(), Is.EqualTo(56.525).Within(0.001));
    }

    [Test]
    public void CalculateTotal_Under50_NoDiscount()
    {
        _cart.AddItem(new ShopItem("Snack", 5.0, 1, "food"));

        // 5 * 1.07 = 5.35, no discount
        Assert.That(_cart.CalculateTotal(), Is.EqualTo(5.35).Within(0.001));
    }

    [Test]
    public void CalculateTotal_MixedItems_CalculatesCorrectly()
    {
        _cart.AddItem(new ShopItem("Bread", 10.0, 2, "food"));
        _cart.AddItem(new ShopItem("Cable", 20.0, 1, "electronics"));

        // Food: 10*2*1.07 = 21.40, Electronics: 20*1*1.19 = 23.80
        // Total: 45.20, no discount
        Assert.That(_cart.CalculateTotal(), Is.EqualTo(45.20).Within(0.001));
    }
}
