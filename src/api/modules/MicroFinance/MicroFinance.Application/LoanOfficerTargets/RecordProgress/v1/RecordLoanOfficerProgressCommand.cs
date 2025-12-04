using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerTargets.RecordProgress.v1;

/// <summary>
/// Command to record progress towards a loan officer target.
/// </summary>
public sealed record RecordLoanOfficerProgressCommand(Guid Id, decimal AchievedValue) : IRequest<RecordLoanOfficerProgressResponse>;
