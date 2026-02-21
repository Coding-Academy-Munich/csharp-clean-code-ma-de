namespace MarsRoverNunitSkTest;

using MarsRoverNunitSk;

[TestFixture]
public class MarsRoverNunitSkTests
{
    [Test]
    public void Test_Message()
    {
        Assert.That(MarsRoverNunitSk.Message, Is.EqualTo("Mars Rover Starter Kit"));
    }
}
