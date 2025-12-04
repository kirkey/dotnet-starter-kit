using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.RecordProgress.v1;

/// <summary>
/// Command to record progress towards a branch target.
/// </summary>
public sealed record RecordBranchProgressCommand(Guid Id, decimal AchievedValue) : IRequest<RecordBranchProgressResponse>;
