namespace RefactoringWithIdeNUnit;

[TestFixture]
public class OrderProcessorTest
{
    private OrderProcessor _processor = null!;

    [SetUp]
    public void SetUp()
    {
        _processor = new OrderProcessor();
    }

    [Test]
    public void CalculateOrderTotal_EmptyOrder_ReturnsZero()
    {
        var order = new Order();

        Assert.That(OrderProcessor.CalculateOrderTotal(order), Is.EqualTo(0m));
    }

    [Test]
    public void CalculateOrderTotal_SingleItem_ReturnsSubtotalPlusTax()
    {
        var order = new Order
        {
            Items = [new OrderItem { Price = 100m, Quantity = 2 }]
        };

        // Subtotal = 200, Tax = 38, Total = 238
        Assert.That(OrderProcessor.CalculateOrderTotal(order), Is.EqualTo(238m));
    }

    [Test]
    public void CalculateOrderTotal_MultipleItems_ReturnsSumPlusTax()
    {
        var order = new Order
        {
            Items =
            [
                new OrderItem { Price = 50m, Quantity = 1 },
                new OrderItem { Price = 30m, Quantity = 3 }
            ]
        };

        // Subtotal = 50 + 90 = 140, Tax = 26.6, Total = 166.6
        Assert.That(OrderProcessor.CalculateOrderTotal(order), Is.EqualTo(166.6m));
    }

    [Test]
    public void ProcessOrders_SetsTotal_OnEachOrder()
    {
        var orders = new List<Order>
        {
            new() { Items = [new OrderItem { Price = 100m, Quantity = 1 }] },
            new() { Items = [new OrderItem { Price = 200m, Quantity = 1 }] }
        };

        _processor.ProcessOrders(orders);

        Assert.That(orders[0].Total, Is.EqualTo(119m));
        Assert.That(orders[1].Total, Is.EqualTo(238m));
    }
}
