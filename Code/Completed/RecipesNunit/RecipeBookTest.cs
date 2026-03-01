namespace RecipesNUnit;

[TestFixture]
public class RecipeBookTest
{
    private RecipeBook unit;

    [SetUp]
    public void SetUp()
    {
        unit = new RecipeBook();
        unit.AddRecipe(new Recipe("recipe 1",
            ["ingredient 1", "ingredient 2"],
            "instructions...",
            5));
        unit.AddRecipe(new Recipe("recipe 2",
            ["ingredient 1", "ingredient 3"],
            "do this...",
            4));
        unit.AddRecipe(new Recipe("recipe 3",
            ["ingredient 2", "ingredient 3"],
            "do that...",
            3));
    }

    [Test]
    public void TestGetRecipeByName()
    {
        Recipe recipe = unit.GetRecipeByName("recipe 2");
        Assert.That(recipe.Name, Is.EqualTo("recipe 2"));
    }

    [Test]
    public void TestGetRecipesWithIngredient()
    {
        List<Recipe> things = unit.GetRecipesWithIngredient("ingredient 2");
        Assert.That(things, Has.Count.EqualTo(2));
        Assert.That(things[0].Name, Is.EqualTo("recipe 1"));
        Assert.That(things[1].Name, Is.EqualTo("recipe 3"));
    }

    [Test]
    public void TestGetRecipesWithRating()
    {
        List<Recipe> things = unit.GetRecipesWithRating(4);
        Assert.That(things, Has.Count.EqualTo(1));
        Assert.That(things[0].Name, Is.EqualTo("recipe 2"));
    }

    [Test]
    public void TestGetRecipesWithRatingAtLeast()
    {
        List<Recipe> things = unit.GetRecipesWithRatingAtLeast(4);
        Assert.That(things, Has.Count.EqualTo(2));
        Assert.That(things[0].Name, Is.EqualTo("recipe 1"));
        Assert.That(things[1].Name, Is.EqualTo("recipe 2"));
    }

    [Test]
    public void GetThingsThrowsStuff()
    {
        Assert.Throws<InvalidOperationException>(() => unit.GetRecipeByName("nonexistent recipe"));
    }
}
