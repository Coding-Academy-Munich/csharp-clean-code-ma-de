# RefactoringWithAiNUnitSk

Starter kit for the AI-assisted refactoring training exercise.

## Purpose

This code is **intentionally bad**. It contains deliberate code smells for students to identify and fix using AI tools:

- `Report.cs` — Long Method, if/else chain, Primitive Obsession, public mutable fields
- `Shop.cs` — Poor naming, tuples instead of types, side effects (`Console.WriteLine`), mutable state

## Running Tests

```bash
dotnet test
```

Tests verify the current behavior. Students should refactor the code while keeping all tests green.

## Notes

- The completed (refactored) version is in `RefactoringWithAiNUnit`
- All assertions use NUnit constraint model (`Assert.That` with `Is.EqualTo`)
- Do not "fix" the code smells in this project — they are intentional training material
