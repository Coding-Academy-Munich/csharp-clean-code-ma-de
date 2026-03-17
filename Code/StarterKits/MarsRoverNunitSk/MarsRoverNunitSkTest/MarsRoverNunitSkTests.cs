namespace MarsRoverNUnitSkTest;

using MarsRoverNUnitSk;

[TestFixture]
public class MarsRoverNUnitSkTests
{
    [Test]
    public void Test_Message()
    {
        Assert.That(MarsRoverNUnitSk.Message, Is.EqualTo("Mars Rover Starter Kit"));
    }
}
