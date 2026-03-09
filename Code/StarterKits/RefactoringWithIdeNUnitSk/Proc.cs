namespace RefactoringWithIdeNUnitSk;

public class Itm
{
    public decimal P { get; set; }
    public int Q { get; set; }
}

public class Ord
{
    public List<Itm> Itms { get; set; } = new();
    public decimal T { get; set; }
}

public class Proc
{
    public void Run(List<Ord> orders)
    {
        foreach (var o in orders)
        {
            decimal s = 0;
            foreach (var i in o.Itms)
            {
                s += i.P * i.Q;
            }
            decimal tx = s * 0.19m;
            decimal t = s + tx;
            o.T = t;
        }
    }
}
