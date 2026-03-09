namespace RefactoringWithIdeNUnit;

public class StatisticsCalculator
{
    public double CalculateMean(List<double> values)
    {
        double sum = 0;
        foreach (var value in values) sum += value;
        return values.Count > 0 ? sum / values.Count : 0;
    }

    public double CalculateVariance(List<double> values)
    {
        double mean = CalculateMean(values);
        double sumOfSquares = 0;
        foreach (var value in values)
            sumOfSquares += (value - mean) * (value - mean);
        return values.Count > 1 ? sumOfSquares / (values.Count - 1) : 0;
    }
}
