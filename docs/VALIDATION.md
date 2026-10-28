# Validation

28 October 2026: `dotnet build Gruberoo.sln` succeeded with .NET SDK 9.0.308 targeting net8.0: zero errors and 81 existing warnings, mainly nullable reference warnings. `dotnet run --no-build --project Gruberoo.csproj` from the project directory with input `0` loaded 15 restaurants, 51 food items, 20 demo customers and 35 orders, displayed the menu and exited successfully.

CSV row counts and cross-file customer references were retained. Transaction dates, amounts, status and line items match the originals; only identity and delivery-location fields were replaced. C# executable source preserves the existing GitHub version after removing student-number comments and renaming the namespace. The local Semester 2 Program.cs differs substantially and contains an unresolved `lat` reference in its reorder draft; it was not used to overwrite the working GitHub version. No business logic was changed.

The build and startup/exit smoke check do not validate every interactive operation or refund calculation. Nullable warnings, input validation, file-write error handling and order-state edge cases remain future improvement areas.
