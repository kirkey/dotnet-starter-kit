# Accrual (Accounting.Domain)

Overview

An Accrual represents an accounting entry that recognizes revenue or expense before cash is exchanged. This aggregate is designed to enforce domain invariants so the rest of the system can rely on a valid, immutable (after reversal) object.

Key rules

- `AccrualNumber` is required, trimmed and limited to 50 characters.
- `Amount` must be greater than zero.
- Once `Reverse` is called the accrual is considered finalized and cannot be updated.
- `Description` is optional, trimmed and truncated to 200 characters.

Fields (properties)

- `AccrualNumber` (string)
  - Purpose: Human-readable identifier for the accrual (e.g., "ACR-2025-0001").
  - Default: empty string for EF/serializer parameterless constructor; on creation the factory will set the trimmed provided value.
  - Validation: required, trimmed, max length 50.

- `AccrualDate` (DateTime)
  - Purpose: The accounting date on which the accrual is recognized.
  - Default: DateTime.MinValue for EF/serializer parameterless constructor; set when created.

- `Amount` (decimal)
  - Purpose: Monetary value of the accrual.
  - Default: 0m for EF/serializer parameterless constructor; must be > 0 when created or updated.
  - Validation: strictly positive.

- `Description` (string?)
  - Purpose: Optional human-readable description.
  - Default: empty string for EF; when set it is trimmed and truncated to 200 characters.

- `IsReversed` (bool)
  - Purpose: Marks whether the accrual was reversed (finalized).
  - Default: false.

- `ReversalDate` (DateTime?)
  - Purpose: When the accrual was reversed. Null if not reversed.

Construction and lifecycle

- Use `Accrual.Create(accrualNumber, accrualDate, amount, description)` to create a new accrual. The factory enforces validation via the private constructor.
- Use `Update(...)` to change mutable fields prior to reversal. `Update` returns the same instance and validates input.
- Use `Reverse(reversalDate)` to mark the accrual as reversed. Once reversed, `Update` will throw `InvalidOperationException`.

Examples

- Creating a new accrual:

  ```csharp
  var accrual = Accrual.Create("ACR-2025-0001", DateTime.UtcNow.Date, 1234.56m, "Accrued revenue for service X");
  ```

- Updating an accrual before reversal:

  ```csharp
  accrual.Update(accrualNumber: null, accrualDate: DateTime.UtcNow.Date, amount: 1300m, description: "Adjusted amount");
  ```

- Reversing an accrual:

  ```csharp
  accrual.Reverse(DateTime.UtcNow);
  // further calls to Update will throw InvalidOperationException
  ```

Notes for developers

- The class includes a parameterless private constructor specifically so EF and serializers can materialize instances.
- The aggregate intentionally returns `this` from `Update` to allow fluent usage in domain code.
- Validation exceptions are thrown as `ArgumentException` or `InvalidOperationException` to clearly indicate misuse.

Contact

If you believe a field must allow nulls or different validation rules (for example, allow zero amounts for adjustments), update the domain methods and tests accordingly and document the change here.

