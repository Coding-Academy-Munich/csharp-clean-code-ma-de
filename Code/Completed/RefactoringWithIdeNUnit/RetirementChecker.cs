namespace RefactoringWithIdeNUnit;

public class RetirementChecker
{
    public static bool IsEligibleForRetirement(int age, int yearsOfService)
    {
        bool isRegularRetirementAge = age >= 65;
        bool isEarlyRetirement = yearsOfService >= 30 && age >= 55;
        return isRegularRetirementAge || isEarlyRetirement;
    }
}
