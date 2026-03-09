namespace RefactoringWithIdeNUnit;

public class BonusCalculator
{
    private readonly List<(string Name, double Salary, int YearsOfService)> _employees = new();

    public void AddEmployee(string name, double salary, int yearsOfService)
        => _employees.Add((name, salary, yearsOfService));

    public double CalculateAverageBonus()
    {
        var eligibleEmployees = _employees
            .Where(e => IsEligibleForBonus(e.YearsOfService, e.Salary));

        if (!eligibleEmployees.Any()) return 0;

        return eligibleEmployees
            .Select(e => CalculateBonus(e.Salary, e.YearsOfService))
            .Average();
    }

    private static bool IsEligibleForBonus(int yearsOfService, double salary)
        => yearsOfService >= 2 && salary > 30000 && salary < 200000;

    private static double CalculateBonus(double salary, int yearsOfService)
    {
        double bonusRate = yearsOfService > 10 ? 0.15
                         : yearsOfService > 5 ? 0.10
                         : 0.05;
        return salary * bonusRate;
    }
}
