using MediatR;

namespace Accounting.Application.Projects.Update;

public record UpdateProjectRequest(
    DefaultIdType Id,
    string? Name = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    decimal? BudgetedAmount = null,
    string? Status = null,
    string? ClientName = null,
    string? ProjectManager = null,
    string? Department = null,
    string? Description = null,
    string? Notes = null) : IRequest<DefaultIdType>;
