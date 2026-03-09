namespace RefactoringWithIdeNUnit;

public class TemperatureConverter
{
    public double CelsiusToFahrenheit(double celsius, int decimalPlaces = 2)
    {
        double result = celsius * 9.0 / 5.0 + 32;
        return Math.Round(result, decimalPlaces);
    }

    public double FahrenheitToCelsius(double fahrenheit, int decimalPlaces = 2)
    {
        double result = (fahrenheit - 32) * 5.0 / 9.0;
        return Math.Round(result, decimalPlaces);
    }

    public double CelsiusToKelvin(double celsius, int decimalPlaces = 2)
    {
        double result = celsius + 273.15;
        return Math.Round(result, decimalPlaces);
    }
}
