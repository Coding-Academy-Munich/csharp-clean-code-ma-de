namespace RefactoringWithIdeNUnit;

public class OrderItem
{
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public class Order
{
    public List<OrderItem> Items { get; set; } = new();
    public decimal Total { get; set; }
}

public class OrderProcessor
{
    public void ProcessOrders(List<Order> orders)
    {
        foreach (var order in orders)
        {
            order.Total = CalculateOrderTotal(order);
        }
    }

    public static decimal CalculateOrderTotal(Order order)
    {
        decimal subtotal = order.Items.Sum(i => i.Price * i.Quantity);
        decimal tax = subtotal * 0.19m;
        return subtotal + tax;
    }
}
