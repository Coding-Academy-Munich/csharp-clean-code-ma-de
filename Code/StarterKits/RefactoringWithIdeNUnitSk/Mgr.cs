namespace RefactoringWithIdeNUnitSk;

public class Mgr
{
    private List<(string n, double s, int y)> emps = new();

    public void Add(string n, double s, int y) => emps.Add((n, s, y));

    public double Proc()
    {
        double t = 0;
        int c = 0;
        foreach (var e in emps)
        {
            if (e.y >= 2 && e.s > 30000 && e.s < 200000)
            {
                double b = e.s * (e.y > 10 ? 0.15 : e.y > 5 ? 0.10 : 0.05);
                t += b;
                c++;
            }
        }
        return c > 0 ? t / c : 0;
    }
}
