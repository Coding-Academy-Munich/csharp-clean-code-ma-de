namespace RefactoringWithIdeNUnitSk;

public class Ship
{
    public decimal Calc(decimal tot, decimal w)
    {
        if (tot >= 100m)
            return 0m;

        decimal b = 4.99m;
        decimal ws = w > 5 ? (w - 5) * 1.50m : 0m;
        return b + ws;
    }
}
