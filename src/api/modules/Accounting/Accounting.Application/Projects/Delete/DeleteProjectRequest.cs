using MediatR;

namespace Accounting.Application.Projects.Delete;

public record DeleteProjectRequest(DefaultIdType Id) : IRequest;
