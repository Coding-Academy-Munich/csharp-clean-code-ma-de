namespace RecipesNunitSk;

[TestFixture]
public class ManyTest
{
    private Many many;

    [SetUp]
    public void SetUp()
    {
        many = new Many();
        many.AddThing(new One("recipe 1",
            ["ingredient 1", "ingredient 2"],
            "instructions...",
            5));
        many.AddThing(new One("recipe 2",
            ["ingredient 1", "ingredient 3"],
            "do this...",
            4));
        many.AddThing(new One("recipe 3",
            ["ingredient 2", "ingredient 3"],
            "do that...",
            3));
    }

    [Test]
    public void GetThing()
    {
        One one = many.GetThing("recipe 2");
        Assert.That(one.Aaa, Is.EqualTo("recipe 2"));
    }

    [Test]
    public void GetThings1Test()
    {
        List<One> things = many.GetThings1("ingredient 2");
        Assert.That(things, Has.Count.EqualTo(2));
        Assert.That(things[0].Aaa, Is.EqualTo("recipe 1"));
        Assert.That(things[1].Aaa, Is.EqualTo("recipe 3"));
    }

    [Test]
    public void GetThings2Test()
    {
        List<One> things = many.GetThings2(4);
        Assert.That(things, Has.Count.EqualTo(1));
        Assert.That(things[0].Aaa, Is.EqualTo("recipe 2"));
    }

    [Test]
    public void GetThings3Test()
    {
        List<One> things = many.GetThings3(4);
        Assert.That(things, Has.Count.EqualTo(2));
        Assert.That(things[0].Aaa, Is.EqualTo("recipe 1"));
        Assert.That(things[1].Aaa, Is.EqualTo("recipe 2"));
    }

    [Test]
    public void GetThingsThrowsStuff()
    {
        Assert.Throws<InvalidOperationException>(() => many.GetThing("nonexistent recipe"));
    }
}
