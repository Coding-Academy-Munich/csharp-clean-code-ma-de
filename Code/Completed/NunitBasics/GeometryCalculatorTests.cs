namespace NUnitBasics;

using static GeometryCalculator;

[TestFixture]
public class GeometryCalculatorTests
{
    [Test]
    public void CalculateCircleArea_ValidRadius_ReturnsCorrectArea()
    {
        // Arrange
        const double radius = 5;

        // Act
        double result = CalculateCircleArea(radius);

        // Assert
        Assert.That(result, Is.EqualTo(78.54).Within(0.01));
    }

    [Test]
    public void CalculateCircleArea_NegativeRadius_ThrowsArgumentException()
    {
        // Arrange
        const double radius = -5;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => CalculateCircleArea(radius));
    }

    [Test]
    public void CalculateRectanglePerimeter_ValidInputs_ReturnsCorrectPerimeter()
    {
        // Arrange
        const double length = 4;
        const double width = 7;

        // Act
        double result = CalculateRectanglePerimeter(length, width);

        // Assert
        Assert.That(result, Is.EqualTo(22));
    }

    [Test]
    public void CalculateRectanglePerimeter_NegativeInputs_ThrowsArgumentException()
    {
        // Arrange
        const double length = -4;
        const double width = 7;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => CalculateRectanglePerimeter(length, width));
    }

    [Test]
    public void CalculateTriangleArea_ValidInputs_ReturnsCorrectArea()
    {
        // Arrange
        const double baseLength = 10;
        const double height = 5;

        // Act
        double result = CalculateTriangleArea(baseLength, height);

        // Assert
        Assert.That(result, Is.EqualTo(25));
    }

    [Test]
    public void CalculateTriangleArea_NegativeInputs_ThrowsArgumentException()
    {
        // Arrange
        const double baseLength = -10;
        const double height = 5;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => CalculateTriangleArea(baseLength, height));
    }

    [Test]
    public void CalculateTriangleArea_ZeroHeight_ReturnsZero()
    {
        // Arrange
        const double baseLength = 10;
        const double height = 0;

        // Act
        double result = CalculateTriangleArea(baseLength, height);

        // Assert
        Assert.That(result, Is.EqualTo(0));
        Assert.That(result == 0, Is.True);
    }
}
