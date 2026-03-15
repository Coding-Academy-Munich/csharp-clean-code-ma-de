# Refactoring Example

## Example Prompt

```claude
This directory contains a legacy system that I would like to improve. I propose the following steps:

- Analyze the functionality of the program
- Build and run the program to see whether the output matches your expectations
- Write characterization tests based on your analysis and the program output
- Improve the program structure and implementation based on the insights gained in the above steps
- Proceed in small steps. Run the tests frequently to ensure that your changes are correct.
- Improve the tests whenever the improved structure of the program allows you to write additional or better tests
```

## Proposed Plan

Here is the plan proposed by Claude Code using Opus 4.6:

---

# Refactoring Legacy Codebase: RefactoringWithAiLegacyNUnitSk

## Context

This project is a legacy C# codebase with intentional code smells and **no tests**. The goal is to:
1. Capture current behavior via characterization tests
2. Refactor incrementally while keeping all tests green
3. Improve tests as structure improves

The completed reference version (`RefactoringWithAiNUnit`) shows the target state. We will work toward that target in small, verifiable steps.

---

## Phase 0: Build & Capture Baseline Output

**Step 0.1** — Run `dotnet run` to verify the program compiles and capture console output as our golden master specification.

Files: `Program.cs` (read-only, used as spec)

---

## Phase 1: Convert Project to NUnit Test Project

**Step 1.1** — Modify `.csproj`: remove `<OutputType>Exe</OutputType>`, add `<IsTestProject>true</IsTestProject>`, add NUnit package references (`NUnit 4.*`, `NUnit3TestAdapter 4.*`, `NUnit.Analyzers 4.*`, `Microsoft.NET.Test.Sdk 17.12.0`), add global `<Using Include="NUnit.Framework"/>`.

**Step 1.2** — Delete `Program.cs` (its behavioral spec is now encoded in our tests).

**Step 1.3** — Run `dotnet build` to verify compilation.

Files modified: `RefactoringWithAiLegacyNUnitSk.csproj`, `Program.cs` (deleted)

---

## Phase 2: Characterization Tests for Report (7 tests)

**Step 2.1** — Create `ReportTest.cs` with 7 tests covering all formats (HTML, CSV, JSON) and edge cases (empty data, single item). Tests use the legacy public-field API (`new Report { type = "html", data = ... }`).

**Step 2.2** — Run `dotnet test` — all 7 pass.

Files created: `ReportTest.cs`

---

## Phase 3: Characterization Tests for Shop (9 tests)

**Step 3.1** — Create `ShopTest.cs` with 9 tests covering: empty cart, each tax category (food 7%, electronics 19%, unknown defaults to 19%), multiple quantity, all three discount tiers (>100, >50, <=50), mixed items. Use `Console.SetOut(StringWriter)` in `[SetUp]`/`[TearDown]` to suppress console output from `CalcTotal()`. Use `.Within(0.001)` for floating-point assertions.

**Step 3.2** — Run `dotnet test` — all 16 pass.

Files created: `ShopTest.cs`

---

## Phase 4: Refactor Report (6 steps)

Each step ends with `dotnet test` — all 16 tests must pass.

**Step 4.1** — Extract `GenerateHtml()`, `GenerateCsv()`, `GenerateJson()` private methods from the if/else chain. Use `string.Join` instead of manual concatenation (behaviorally equivalent).

**Step 4.2** — Introduce `IReportFormatter` interface with `string Format(List<string> data)`.

**Step 4.3** — Create `HtmlFormatter`, `CsvFormatter`, `JsonFormatter` classes implementing `IReportFormatter`, moving logic from the private methods.

**Step 4.4** — Add constructor `Report(List<string> data, IReportFormatter formatter)` that stores formatter in a private field. `Generate()` uses formatter if available, otherwise falls back to old logic.

**Step 4.5** — Migrate all `ReportTest.cs` tests to use the new constructor with formatter objects.

**Step 4.6** — Remove old API: delete public `type`/`data` fields, parameterless constructor, old if/else logic, private format methods. Rename `sent` to `HasBeenSent` property.

Files modified: `Report.cs`, `ReportTest.cs`

---

## Phase 5: Refactor Shop (8 steps)

Each step ends with `dotnet test` — all 16 tests must pass.

**Step 5.1** — Introduce `ShopItem` record: `public record ShopItem(string Name, double Price, int Quantity, string Category)`.

**Step 5.2** — Add `AddItem(ShopItem)` method to `Shop` that internally converts to tuple.

**Step 5.3** — Migrate all `ShopTest.cs` tests to use `AddItem(new ShopItem(...))` instead of `shop.items.Add(tuple)`.

**Step 5.4** — Extract `CalculateItemTotal` and compute discount locally inside `CalcTotal()`.

**Step 5.5** — Replace internal tuple list with `List<ShopItem>`. Update `AddItem` and `CalcTotal` accordingly.

**Step 5.6** — Remove `Console.WriteLine` from `CalcTotal()`. Then remove console redirection from `ShopTest.cs` (`[SetUp]`/`[TearDown]`).

**Step 5.7** — Make `items` private, remove public `disc` field (compute locally). Rename `Shop` → `ShoppingCart`, `CalcTotal` → `CalculateTotal`. Update tests. Add `[SetUp]` with shared `_cart` field.

**Step 5.8** — Introduce tax rate dictionary + `DefaultTaxRate` constant. Use LINQ `_items.Sum(...)`. Extract `DetermineDiscountRate` method.

Files modified: `Shop.cs` (→ renamed conceptually to ShoppingCart), `ShopTest.cs` (→ `ShoppingCartTest.cs`)

---

## Phase 6: Final Verification

**Step 6.1** — Run `dotnet test` — all 16 tests pass.
**Step 6.2** — Review final code against completed reference version for consistency.

---

## Verification

After every step: `dotnet test` must show all tests passing. Final state: 16 tests (7 Report + 9 ShoppingCart).
