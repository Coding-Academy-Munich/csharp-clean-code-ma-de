using System.Globalization;

namespace RefactoringWithIdeNUnitSk;

[TestFixture]
public class FmtTest
{
    private Fmt _fmt = null!;

    [SetUp]
    public void SetUp()
    {
        _fmt = new Fmt();
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
    }

    [Test]
    public void Fmt1_SingleItem_ContainsDescriptionAndTotal()
    {
        var result = _fmt.Fmt1("Widget", 10.00m, 3);

        Assert.That(result, Does.Contain("Widget"));
        Assert.That(result, Does.Contain("¤30.00"));
    }

    [Test]
    public void Fmt2_SingleItem_ShowsCorrectTaxAndTotal()
    {
        var items = new List<(string, decimal, int)> { ("Widget", 100m, 1) };

        var result = _fmt.Fmt2(items);

        Assert.That(result, Does.Contain("¤100.00"));
        Assert.That(result, Does.Contain("¤19.00"));
        Assert.That(result, Does.Contain("¤119.00"));
    }

    [Test]
    public void Fmt2_MultipleItems_SumsCorrectly()
    {
        var items = new List<(string, decimal, int)>
        {
            ("Widget", 50m, 2),
            ("Gadget", 75m, 1)
        };

        var result = _fmt.Fmt2(items);

        Assert.That(result, Does.Contain("¤175.00"));
        Assert.That(result, Does.Contain("¤33.25"));
        Assert.That(result, Does.Contain("¤208.25"));
    }
}
