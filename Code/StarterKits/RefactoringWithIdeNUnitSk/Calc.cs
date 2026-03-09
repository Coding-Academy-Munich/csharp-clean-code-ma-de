namespace RefactoringWithIdeNUnitSk;

public class Calc
{
    public double Calc1(List<double> d)
    {
        double s = 0;
        foreach (var x in d) s += x;
        return d.Count > 0 ? s / d.Count : 0;
    }

    public double Calc2(List<double> d)
    {
        double s = 0;
        foreach (var x in d)
            s += (x - Calc1(d)) * (x - Calc1(d));
        return d.Count > 1 ? s / (d.Count - 1) : 0;
    }
}
