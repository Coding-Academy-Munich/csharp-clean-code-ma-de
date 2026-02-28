using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;

public class NUnitTestRunner
{
    public static void RunTests<TTestClass>() where TTestClass : class
    {
        RunTests(typeof(TTestClass));
    }

    public static void RunTests(Type testClassType)
    {
        int totalTests = 0;
        int passedTests = 0;
        int failedTests = 0;

        // Find [OneTimeSetUp] and [OneTimeTearDown] methods
        var oneTimeSetUpMethods = testClassType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(m => m.GetCustomAttributes(typeof(OneTimeSetUpAttribute), true).Any())
            .ToList();

        var oneTimeTearDownMethods = testClassType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(m => m.GetCustomAttributes(typeof(OneTimeTearDownAttribute), true).Any())
            .ToList();

        // Find [SetUp] and [TearDown] methods
        var setUpMethods = testClassType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(m => m.GetCustomAttributes(typeof(SetUpAttribute), true).Any())
            .ToList();

        var tearDownMethods = testClassType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(m => m.GetCustomAttributes(typeof(TearDownAttribute), true).Any())
            .ToList();

        // Find test methods: [Test] or [TestCase]
        var testMethods = testClassType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Where(m => m.GetCustomAttributes(typeof(TestAttribute), true).Any() ||
                        m.GetCustomAttributes(typeof(TestCaseAttribute), true).Any())
            .ToList();

        // Create a shared instance for [OneTimeSetUp] / [OneTimeTearDown]
        var sharedInstance = Activator.CreateInstance(testClassType);

        // Run [OneTimeSetUp]
        foreach (var method in oneTimeSetUpMethods)
        {
            method.Invoke(sharedInstance, null);
        }

        foreach (var method in testMethods)
        {
            var testCaseAttributes = method.GetCustomAttributes(typeof(TestCaseAttribute), true)
                .Cast<TestCaseAttribute>().ToList();

            if (testCaseAttributes.Any())
            {
                // Handle [TestCase] methods with inline data
                foreach (var testCase in testCaseAttributes)
                {
                    totalTests++;
                    var testInstance = Activator.CreateInstance(testClassType);
                    CopyOneTimeSetUpState(sharedInstance, testInstance, testClassType);

                    try
                    {
                        foreach (var setUp in setUpMethods) setUp.Invoke(testInstance, null);
                        method.Invoke(testInstance, testCase.Arguments);
                        Console.WriteLine($"[PASS] {method.Name}({FormatParameters(testCase.Arguments)})");
                        passedTests++;
                    }
                    catch (TargetInvocationException ex)
                    {
                        Console.WriteLine($"[FAIL] {method.Name}({FormatParameters(testCase.Arguments)}): {ex.InnerException?.Message ?? ex.Message}");
                        failedTests++;
                    }
                    finally
                    {
                        foreach (var tearDown in tearDownMethods)
                        {
                            try { tearDown.Invoke(testInstance, null); } catch { }
                        }
                        if (testInstance is IDisposable disposable) disposable.Dispose();
                    }
                }
            }
            else
            {
                // Handle [Test] methods without data
                totalTests++;
                var testInstance = Activator.CreateInstance(testClassType);
                CopyOneTimeSetUpState(sharedInstance, testInstance, testClassType);

                try
                {
                    foreach (var setUp in setUpMethods) setUp.Invoke(testInstance, null);
                    method.Invoke(testInstance, null);
                    Console.WriteLine($"[PASS] {method.Name}");
                    passedTests++;
                }
                catch (TargetInvocationException ex)
                {
                    Console.WriteLine($"[FAIL] {method.Name}: {ex.InnerException?.Message ?? ex.Message}");
                    failedTests++;
                }
                finally
                {
                    foreach (var tearDown in tearDownMethods)
                    {
                        try { tearDown.Invoke(testInstance, null); } catch { }
                    }
                    if (testInstance is IDisposable disposable) disposable.Dispose();
                }
            }
        }

        // Run [OneTimeTearDown]
        foreach (var method in oneTimeTearDownMethods)
        {
            try { method.Invoke(sharedInstance, null); } catch { }
        }
        if (sharedInstance is IDisposable disposableShared) disposableShared.Dispose();

        Console.WriteLine($"Total tests: {totalTests}, Passed: {passedTests}, Failed: {failedTests}");
    }

    private static void CopyOneTimeSetUpState(object source, object target, Type type)
    {
        // Copy field values from the shared instance (which ran OneTimeSetUp) to the test instance
        foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            field.SetValue(target, field.GetValue(source));
        }
    }

    private static string FormatParameters(object[] parameters)
    {
        if (parameters == null)
            return string.Empty;
        return string.Join(", ", parameters.Select(p => p == null ? "null" : p.ToString()));
    }
}
