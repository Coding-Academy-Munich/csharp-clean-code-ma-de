namespace RefactoringWithIdeNUnitSk;

public class Conv
{
    public double C2f(double v)
    {
        return Math.Round(v * 9.0 / 5.0 + 32, 2);
    }

    public double F2c(double v)
    {
        return Math.Round((v - 32) * 5.0 / 9.0, 2);
    }

    public double C2k(double v)
    {
        return Math.Round(v + 273.15, 2);
    }
}
