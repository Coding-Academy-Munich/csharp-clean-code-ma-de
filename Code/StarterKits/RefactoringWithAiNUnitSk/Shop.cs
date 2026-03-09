namespace RefactoringWithAiNUnitSk;

public class Shop
{
    public List<(string name, double price, int qty, string cat)> items = new();
    public double disc = 0;

    public double CalcTotal()
    {
        double t = 0;
        for (int i = 0; i < items.Count; i++)
        {
            double p = items[i].price * items[i].qty;
            if (items[i].cat == "food")
                p = p * 1.07;
            else if (items[i].cat == "electronics")
                p = p * 1.19;
            else
                p = p * 1.19;
            t += p;
        }
        if (t > 100) disc = 0.1;
        else if (t > 50) disc = 0.05;
        else disc = 0;
        t = t * (1 - disc);
        Console.WriteLine($"Total: {t:C}");
        return t;
    }
}
