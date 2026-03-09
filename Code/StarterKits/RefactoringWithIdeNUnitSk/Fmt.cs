namespace RefactoringWithIdeNUnitSk;

public class Fmt
{
    private decimal GetTr()
    {
        return 0.19m;
    }

    private string GetLbl(int t)
    {
        return t == 1 ? "Subtotal" : t == 2 ? "Tax" : "Total";
    }

    public string Fmt1(string d, decimal p, int q)
    {
        decimal lt = p * q;
        return $"{d,-30} {q,5} x {p,10:C} = {lt,12:C}";
    }

    public string Fmt2(List<(string d, decimal p, int q)> items)
    {
        decimal t = 0;
        foreach (var i in items) t += i.p * i.q;
        decimal tx = t * GetTr();
        return $"{GetLbl(1)}: {t:C}\n{GetLbl(2)} ({GetTr() * 100}%): {tx:C}\n{GetLbl(3)}: {t + tx:C}";
    }
}
