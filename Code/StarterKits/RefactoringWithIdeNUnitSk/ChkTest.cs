namespace RefactoringWithIdeNUnitSk;

[TestFixture]
public class ChkTest
{
    [Test]
    public void Chk1_Age65_ReturnsTrue()
    {
        Assert.That(Chk.Chk1(65, 5), Is.True);
    }

    [Test]
    public void Chk1_Age70_ReturnsTrue()
    {
        Assert.That(Chk.Chk1(70, 0), Is.True);
    }

    [Test]
    public void Chk1_Age55With30Years_ReturnsTrue()
    {
        Assert.That(Chk.Chk1(55, 30), Is.True);
    }

    [Test]
    public void Chk1_Age60With35Years_ReturnsTrue()
    {
        Assert.That(Chk.Chk1(60, 35), Is.True);
    }

    [Test]
    public void Chk1_Age50With20Years_ReturnsFalse()
    {
        Assert.That(Chk.Chk1(50, 20), Is.False);
    }

    [Test]
    public void Chk1_Age54With30Years_ReturnsFalse()
    {
        Assert.That(Chk.Chk1(54, 30), Is.False);
    }

    [Test]
    public void Chk1_Age55With29Years_ReturnsFalse()
    {
        Assert.That(Chk.Chk1(55, 29), Is.False);
    }
}
