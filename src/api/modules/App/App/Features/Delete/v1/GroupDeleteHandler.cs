using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.App.Domain;
using FSH.Starter.WebApi.App.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.App.Features.Delete.v1;

public sealed class GroupDeleteHandler(
    ILogger<GroupDeleteHandler> logger,
    [FromKeyedServices("Group")] IRepository<Group> repository)
    : IRequestHandler<GroupDeleteCommand>
{
    public async Task Handle(GroupDeleteCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var group = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = group ?? throw new GroupNotFoundException(request.Id);
        await repository.DeleteAsync(group, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("app group with id : {AppId} deleted", group.Id);
    }
}
