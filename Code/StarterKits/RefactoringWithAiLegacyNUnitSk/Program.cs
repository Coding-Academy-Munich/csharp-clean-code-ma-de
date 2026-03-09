namespace RefactoringWithAiLegacyNUnitSk;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Report: HTML Format ===");
        var htmlReport = new Report { type = "html", data = new List<string> { "Alice", "Bob" } };
        Console.WriteLine(htmlReport.Generate());

        Console.WriteLine("\n=== Report: CSV Format ===");
        var csvReport = new Report { type = "csv", data = new List<string> { "Alice", "Bob", "Charlie" } };
        Console.WriteLine(csvReport.Generate());

        Console.WriteLine("\n=== Report: JSON Format ===");
        var jsonReport = new Report { type = "json", data = new List<string> { "Alice", "Bob" } };
        Console.WriteLine(jsonReport.Generate());

        Console.WriteLine("\n=== Report: Empty Data (HTML) ===");
        var emptyHtml = new Report { type = "html", data = new List<string>() };
        Console.WriteLine(emptyHtml.Generate());

        Console.WriteLine("\n=== Report: Empty Data (CSV) ===");
        var emptyCsv = new Report { type = "csv", data = new List<string>() };
        Console.WriteLine($"'{emptyCsv.Generate()}'");

        Console.WriteLine("\n=== Report: Empty Data (JSON) ===");
        var emptyJson = new Report { type = "json", data = new List<string>() };
        Console.WriteLine(emptyJson.Generate());

        Console.WriteLine("\n=== Report: Single Item (CSV) ===");
        var singleCsv = new Report { type = "csv", data = new List<string> { "Only" } };
        Console.WriteLine(singleCsv.Generate());

        Console.WriteLine("\n=== Shop: Empty Cart ===");
        var emptyShop = new Shop();
        Console.WriteLine($"Result: {emptyShop.CalcTotal()}");

        Console.WriteLine("\n=== Shop: Single Food Item (7% tax) ===");
        var foodShop = new Shop();
        foodShop.items.Add(("Bread", 10.0, 1, "food"));
        Console.WriteLine($"Result: {foodShop.CalcTotal()}");

        Console.WriteLine("\n=== Shop: Single Electronics Item (19% tax) ===");
        var electronicsShop = new Shop();
        electronicsShop.items.Add(("Cable", 10.0, 1, "electronics"));
        Console.WriteLine($"Result: {electronicsShop.CalcTotal()}");

        Console.WriteLine("\n=== Shop: Unknown Category (default tax) ===");
        var unknownShop = new Shop();
        unknownShop.items.Add(("Book", 10.0, 1, "books"));
        Console.WriteLine($"Result: {unknownShop.CalcTotal()}");

        Console.WriteLine("\n=== Shop: Multiple Quantity ===");
        var qtyShop = new Shop();
        qtyShop.items.Add(("Apple", 5.0, 3, "food"));
        Console.WriteLine($"Result: {qtyShop.CalcTotal()}");

        Console.WriteLine("\n=== Shop: Over 100 (10% discount) ===");
        var bigShop = new Shop();
        bigShop.items.Add(("Laptop", 100.0, 1, "electronics"));
        Console.WriteLine($"Result: {bigShop.CalcTotal()}, Discount: {bigShop.disc}");

        Console.WriteLine("\n=== Shop: Over 50 (5% discount) ===");
        var midShop = new Shop();
        midShop.items.Add(("Headphones", 50.0, 1, "electronics"));
        Console.WriteLine($"Result: {midShop.CalcTotal()}, Discount: {midShop.disc}");

        Console.WriteLine("\n=== Shop: Under 50 (no discount) ===");
        var smallShop = new Shop();
        smallShop.items.Add(("Snack", 5.0, 1, "food"));
        Console.WriteLine($"Result: {smallShop.CalcTotal()}, Discount: {smallShop.disc}");

        Console.WriteLine("\n=== Shop: Mixed Items ===");
        var mixedShop = new Shop();
        mixedShop.items.Add(("Bread", 10.0, 2, "food"));
        mixedShop.items.Add(("Cable", 20.0, 1, "electronics"));
        Console.WriteLine($"Result: {mixedShop.CalcTotal()}, Discount: {mixedShop.disc}");
    }
}
