using System.Globalization;

namespace RefactoringWithIdeNUnit;

[TestFixture]
public class InvoiceFormatterTest
{
    private InvoiceFormatter _formatter = null!;

    [SetUp]
    public void SetUp()
    {
        _formatter = new InvoiceFormatter();
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
    }

    [Test]
    public void FormatInvoiceLine_SingleItem_ContainsDescriptionAndTotal()
    {
        var result = _formatter.FormatInvoiceLine("Widget", 10.00m, 3);

        Assert.That(result, Does.Contain("Widget"));
        Assert.That(result, Does.Contain("¤30.00"));
    }

    [Test]
    public void FormatTotal_SingleItem_ShowsCorrectTaxAndTotal()
    {
        var items = new List<(string, decimal, int)> { ("Widget", 100m, 1) };

        var result = _formatter.FormatTotal(items);

        Assert.That(result, Does.Contain("¤100.00"));   // Subtotal
        Assert.That(result, Does.Contain("¤19.00"));    // Tax
        Assert.That(result, Does.Contain("¤119.00"));   // Total
    }

    [Test]
    public void FormatTotal_MultipleItems_SumsCorrectly()
    {
        var items = new List<(string, decimal, int)>
        {
            ("Widget", 50m, 2),
            ("Gadget", 75m, 1)
        };

        var result = _formatter.FormatTotal(items);

        // Subtotal = 100 + 75 = 175, Tax = 33.25, Total = 208.25
        Assert.That(result, Does.Contain("¤175.00"));
        Assert.That(result, Does.Contain("¤33.25"));
        Assert.That(result, Does.Contain("¤208.25"));
    }
}
