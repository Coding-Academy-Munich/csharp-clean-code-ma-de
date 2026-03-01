namespace RecipesNUnitSk;

[TestFixture]
public class OneTest
{
    [Test]
    public void ThingDddIsMinusOne()
    {
        var recipe = new One("Test Recipe", [], "");
        Assert.That(recipe.Ddd, Is.EqualTo(-1));
    }
}
