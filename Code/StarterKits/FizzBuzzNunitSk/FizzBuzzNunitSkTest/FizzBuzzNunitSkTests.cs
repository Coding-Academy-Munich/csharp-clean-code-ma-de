namespace FizzBuzzNunitSkTest;

[TestFixture]
public class FizzBuzzNunitSkTests
{
    [Test]
    public void Test_Message()
    {
        Assert.That(FizzBuzzNunitSk.FizzBuzzNunitSk.Message, Is.EqualTo("FizzBuzz Starter Kit"));
    }
}
