using MediatR;

namespace FSH.Starter.WebApi.App.Features.Delete.v1;

public sealed record GroupDeleteCommand(
    DefaultIdType Id) : IRequest;
