namespace Accounting.Application.RetainedEarnings.Close.v1;

/// <summary>
/// Command to close a retained earnings fiscal year.
/// The user who closes the fiscal year is automatically determined from the current user session.
/// </summary>
public sealed record CloseRetainedEarningsCommand(DefaultIdType Id) : IRequest<DefaultIdType>;
