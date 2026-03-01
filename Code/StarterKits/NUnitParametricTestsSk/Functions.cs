namespace NUnitParametricTestsSk;

public static class Functions
{
    public static bool IsLeapYear(int year)
    {
        return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
    }

    public static bool IsPrime(int n)
    {
        if (n <= 1)
        {
            return false;
        }
        for (int i = 2; i <= Math.Sqrt(n); i++)
        {
            if (n % i == 0)
            {
                return false;
            }
        }
        return true;
    }

    public static bool IsPalindrome(string s)
    {
        return s.Equals(new string(s.Reverse().ToArray()));
    }

    public static bool ContainsDigit(int n, int digit)
    {
        return n.ToString().Contains(digit.ToString());
    }

    public static string SubstringFollowing(string s, string prefix)
    {
        int index = s.IndexOf(prefix);
        if (index == -1)
        {
            return s;
        }
        return s.Substring(index + prefix.Length);
    }
}
