namespace RecipesNunit;

[TestFixture]
public class RecipeTest
{
    [Test]
    public void RecipeHasNoRatingByDefault()
    {
        var recipe = new Recipe("Test Recipe", [], "");
        Assert.That(recipe.HasRating, Is.False);
    }

    [Test]
    public void RecipeHasRatingWhenProvidedToConstructor()
    {
        var recipe = new Recipe("Test Recipe", [], "", 5);
        Assert.That(recipe.HasRating, Is.True);
    }
}
