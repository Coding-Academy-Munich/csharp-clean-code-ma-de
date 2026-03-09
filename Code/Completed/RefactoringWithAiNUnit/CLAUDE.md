# RefactoringWithAiNUnit

Completed (refactored) version of the AI-assisted refactoring training exercise.

## Structure

- `Report.cs` — Strategy pattern: `IReportFormatter` interface with `HtmlFormatter`, `CsvFormatter`, `JsonFormatter`
- `ShoppingCart.cs` — `ShopItem` record + `ShoppingCart` class with extracted methods and dictionary-based tax rates
- `ReportTest.cs` / `ShoppingCartTest.cs` — NUnit tests (constraint-model `Assert.That`)

## Running Tests

```bash
dotnet test
```

## Notes

- The starter kit (`RefactoringWithAiNUnitSk`) contains the original code with intentional code smells
- Students use AI tools to refactor the starter kit toward this completed version
- All assertions use NUnit constraint model (`Assert.That` with `Is.EqualTo`)
