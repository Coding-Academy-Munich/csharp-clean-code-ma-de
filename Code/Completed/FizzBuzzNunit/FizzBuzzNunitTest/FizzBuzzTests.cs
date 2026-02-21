namespace FizzBuzzNunitTest;

[TestFixture]
public class FizzBuzzTests
{
    [Test]
    public void GenerateFizzBuzz_ReturnsCorrectSequenceFor15()
    {
        var expected = new List<string>
        {
            "1", "2", "Fizz", "4", "Buzz", "Fizz", "7", "8", "Fizz", "Buzz",
            "11", "Fizz", "13", "14", "FizzBuzz"
        };
        var result = FizzBuzzNunit.FizzBuzz.GenerateFizzBuzz(15);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase(1, "1")]
    [TestCase(3, "Fizz")]
    [TestCase(5, "Buzz")]
    [TestCase(15, "FizzBuzz")]
    public void GenerateFizzBuzz_ReturnsCorrectValueForSpecificNumbers(int number, string expected)
    {
        var result = FizzBuzzNunit.FizzBuzz.GenerateFizzBuzz(number);
        Assert.That(result[^1], Is.EqualTo(expected));
    }

    [Test]
    public void GenerateFizzBuzz_ReturnsEmptyListForZero()
    {
        var result = FizzBuzzNunit.FizzBuzz.GenerateFizzBuzz(0);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GenerateFizzBuzz_ThrowsArgumentExceptionForNegativeNumber()
    {
        Assert.Throws<ArgumentException>(() => FizzBuzzNunit.FizzBuzz.GenerateFizzBuzz(-1));
    }

    [Test]
    public void GenerateFizzBuzz_ReturnsCorrectSequenceFor100()
    {
        var result = FizzBuzzNunit.FizzBuzz.GenerateFizzBuzz(100);
        Assert.That(result, Has.Count.EqualTo(100));
        Assert.That(result[0], Is.EqualTo("1"));
        Assert.That(result[1], Is.EqualTo("2"));
        Assert.That(result[2], Is.EqualTo("Fizz"));
        Assert.That(result[3], Is.EqualTo("4"));
        Assert.That(result[4], Is.EqualTo("Buzz"));
        Assert.That(result[5], Is.EqualTo("Fizz"));
        Assert.That(result[14], Is.EqualTo("FizzBuzz"));
        Assert.That(result[29], Is.EqualTo("FizzBuzz"));
        Assert.That(result[44], Is.EqualTo("FizzBuzz"));
        Assert.That(result[99], Is.EqualTo("Buzz"));
    }

    [Test]
    public void PrintFizzBuzz_WritesToProvidedTextWriter()
    {
        using var stringWriter = new StringWriter();
        FizzBuzzNunit.FizzBuzz.PrintFizzBuzz(5, stringWriter);
        var result = stringWriter.ToString().Trim().Split(Environment.NewLine);
        Assert.That(result, Is.EqualTo(new[] { "1", "2", "Fizz", "4", "Buzz" }));
    }

    [Test]
    public void PrintFizzBuzzToConsole_WritesToConsoleOut()
    {
        var originalOut = Console.Out;
        try
        {
            using var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            FizzBuzzNunit.FizzBuzz.PrintFizzBuzz(3);
            var result = stringWriter.ToString().Trim().Split(Environment.NewLine);
            Assert.That(result, Is.EqualTo(new[] { "1", "2", "Fizz" }));
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
}
