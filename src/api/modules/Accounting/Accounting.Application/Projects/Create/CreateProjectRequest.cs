using MediatR;

namespace Accounting.Application.Projects.Create;

public record CreateProjectRequest(
    string Name,
    DateTime StartDate,
    decimal BudgetedAmount,
    string? ClientName = null,
    string? ProjectManager = null,
    string? Department = null,
    string? Description = null,
    string? Notes = null) : IRequest<DefaultIdType>;
