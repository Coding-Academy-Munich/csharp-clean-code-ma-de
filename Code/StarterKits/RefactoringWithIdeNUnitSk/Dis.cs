namespace RefactoringWithIdeNUnitSk;

public class Dis
{
    public decimal Calc(decimal p, int q)
    {
        decimal t = p * q;
        if (t > 100m)
            t -= 10m;
        return Math.Round(t, 2);
    }
}
