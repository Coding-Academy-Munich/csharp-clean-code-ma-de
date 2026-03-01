namespace FizzBuzzNUnitSkTest;

[TestFixture]
public class FizzBuzzNUnitSkTests
{
    [Test]
    public void Test_Message()
    {
        Assert.That(FizzBuzzNUnitSk.FizzBuzzNUnitSk.Message, Is.EqualTo("FizzBuzz Starter Kit"));
    }
}
