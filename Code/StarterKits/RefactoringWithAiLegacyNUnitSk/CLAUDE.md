# RefactoringWithAiLegacyNUnitSk

Legacy codebase starter kit for AI-assisted test writing and refactoring.

## Purpose

This project simulates a **legacy codebase without tests**. The code is intentionally bad
and contains many code smells:

- `Report.cs` — Long Method, if/else chain, Primitive Obsession, public mutable fields
- `Shop.cs` — Poor naming, tuples instead of types, side effects (`Console.WriteLine`), mutable state
- `Program.cs` — Exercises all code paths (serves as informal specification)

## Exercise

Use an AI coding tool (e.g., Claude Code) to:

1. **Write NUnit tests** that capture the current behavior of `Report` and `Shop`
2. **Verify** the tests pass against the existing code
3. **Refactor** the code while keeping all tests green

This mirrors a real-world workflow: inheriting legacy code, adding a test harness, then
safely improving the design.

## Running the Program

```bash
dotnet run
```

## Notes

- There are **no tests** in this project — writing them is part of the exercise
- The completed (refactored) version with tests is in `RefactoringWithAiNUnit`
- When adding NUnit tests, you will need to convert this to a test project or add a
  separate test project
- Do not "fix" the code smells before writing tests — they are intentional training material
