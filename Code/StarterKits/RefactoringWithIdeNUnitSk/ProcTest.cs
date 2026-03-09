namespace RefactoringWithIdeNUnitSk;

[TestFixture]
public class ProcTest
{
    private Proc _proc = null!;

    [SetUp]
    public void SetUp()
    {
        _proc = new Proc();
    }

    [Test]
    public void Run_EmptyOrder_SetsZero()
    {
        var orders = new List<Ord> { new() };

        _proc.Run(orders);

        Assert.That(orders[0].T, Is.EqualTo(0m));
    }

    [Test]
    public void Run_SingleItem_SetsSubtotalPlusTax()
    {
        var orders = new List<Ord>
        {
            new() { Itms = [new Itm { P = 100m, Q = 2 }] }
        };

        _proc.Run(orders);

        Assert.That(orders[0].T, Is.EqualTo(238m));
    }

    [Test]
    public void Run_MultipleItems_SetsSumPlusTax()
    {
        var orders = new List<Ord>
        {
            new()
            {
                Itms =
                [
                    new Itm { P = 50m, Q = 1 },
                    new Itm { P = 30m, Q = 3 }
                ]
            }
        };

        _proc.Run(orders);

        Assert.That(orders[0].T, Is.EqualTo(166.6m));
    }

    [Test]
    public void Run_MultipleOrders_SetsEachTotal()
    {
        var orders = new List<Ord>
        {
            new() { Itms = [new Itm { P = 100m, Q = 1 }] },
            new() { Itms = [new Itm { P = 200m, Q = 1 }] }
        };

        _proc.Run(orders);

        Assert.That(orders[0].T, Is.EqualTo(119m));
        Assert.That(orders[1].T, Is.EqualTo(238m));
    }
}
