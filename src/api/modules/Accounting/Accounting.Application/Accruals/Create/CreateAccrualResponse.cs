namespace Accounting.Application.Accruals.Create;

/// <summary>
/// Response returned after creating an accrual, containing the new Accrual Id.
/// </summary>
/// <param name="Id">Identifier of the created Accrual.</param>
public sealed record CreateAccrualResponse(DefaultIdType Id);

