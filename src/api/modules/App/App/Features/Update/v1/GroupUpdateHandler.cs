using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.App.Domain;
using FSH.Starter.WebApi.App.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.App.Features.Update.v1;

public sealed class GroupUpdateHandler(
    ILogger<GroupUpdateHandler> logger,
    [FromKeyedServices("Group")] IRepository<Group> repository)
    : IRequestHandler<GroupUpdateCommand, GroupUpdateResponse>
{
    public async Task<GroupUpdateResponse> Handle(GroupUpdateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var app = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = app ?? throw new GroupNotFoundException(request.Id);
        var updatedApp = app.Update(request.Application, request.Parent, request.Tag, request.Number,
            request.Code, request.Name, request.Amount, request.EmployeeId, request.EmployeeName,
            request.Description, request.Notes);
        await repository.UpdateAsync(updatedApp, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("app group updated {GroupId}", updatedApp.Id);
        return new GroupUpdateResponse(updatedApp.Id);
    }
}
