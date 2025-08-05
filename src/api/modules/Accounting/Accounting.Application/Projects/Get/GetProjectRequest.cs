using MediatR;
using Accounting.Application.Projects.Dtos;

namespace Accounting.Application.Projects.Get;

public record GetProjectRequest(DefaultIdType Id) : IRequest<ProjectDto>;
