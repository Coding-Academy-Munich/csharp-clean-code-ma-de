namespace RefactoringWithIdeNUnit;

[TestFixture]
public class TemperatureConverterTest
{
    private TemperatureConverter _converter = null!;

    [SetUp]
    public void SetUp()
    {
        _converter = new TemperatureConverter();
    }

    [Test]
    public void CelsiusToFahrenheit_FreezingPoint_Returns32()
    {
        Assert.That(_converter.CelsiusToFahrenheit(0), Is.EqualTo(32.0));
    }

    [Test]
    public void CelsiusToFahrenheit_BoilingPoint_Returns212()
    {
        Assert.That(_converter.CelsiusToFahrenheit(100), Is.EqualTo(212.0));
    }

    [Test]
    public void CelsiusToFahrenheit_WithPrecision_RoundsCorrectly()
    {
        Assert.That(_converter.CelsiusToFahrenheit(37.778, 1), Is.EqualTo(100.0));
    }

    [Test]
    public void FahrenheitToCelsius_FreezingPoint_ReturnsZero()
    {
        Assert.That(_converter.FahrenheitToCelsius(32), Is.EqualTo(0.0));
    }

    [Test]
    public void FahrenheitToCelsius_BoilingPoint_Returns100()
    {
        Assert.That(_converter.FahrenheitToCelsius(212), Is.EqualTo(100.0));
    }

    [Test]
    public void FahrenheitToCelsius_BodyTemperature_RoundsToTwoDecimals()
    {
        Assert.That(_converter.FahrenheitToCelsius(98.6), Is.EqualTo(37.0));
    }

    [Test]
    public void CelsiusToKelvin_AbsoluteZero_Returns0()
    {
        Assert.That(_converter.CelsiusToKelvin(-273.15), Is.EqualTo(0.0));
    }

    [Test]
    public void CelsiusToKelvin_FreezingPoint_Returns273Point15()
    {
        Assert.That(_converter.CelsiusToKelvin(0), Is.EqualTo(273.15));
    }
}
