using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.App.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.App.Features.Create.v1;

public sealed class GroupCreateHandler(
    ILogger<GroupCreateHandler> logger,
    [FromKeyedServices("Group")] IRepository<Group> repository)
    : IRequestHandler<GroupCreateCommand, GroupCreateResponse>
{
    public async Task<GroupCreateResponse> Handle(GroupCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = Group.Create(request.Application, request.Parent, request.Tag, request.Number, request.Code,
            request.Name, request.Amount, request.EmployeeId, request.EmployeeName,
            request.Description, request.Notes);
        await repository.AddAsync(item, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("app group created {GroupId}", item.Id);
        return new GroupCreateResponse(item.Id);
    }
}
