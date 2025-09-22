namespace Accounting.Application.Accruals.Delete;

/// <summary>
/// Command to delete an accrual by identifier.
/// </summary>
public sealed record DeleteAccrualCommand(DefaultIdType Id) : IRequest;
