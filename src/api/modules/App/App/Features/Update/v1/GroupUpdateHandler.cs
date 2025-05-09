using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.App.Domain;
using FSH.Starter.WebApi.App.Exceptions;
using FSH.Starter.WebApi.App.Features.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.App.Features.Update.v1;

public sealed class GroupUpdateHandler(
    ILogger<GroupUpdateHandler> logger,
    [FromKeyedServices("app:group")] IRepository<Group> repository)
    : IRequestHandler<GroupUpdateCommand, GroupUpdateResponse>
{
    public async Task<GroupUpdateResponse> Handle(GroupUpdateCommand request, CancellationToken cancellationToken)
    {
        var existingGroupByCode = await repository.SingleOrDefaultAsync(new GroupByCodeSpec(request.Code), cancellationToken);
        if (existingGroupByCode != null && existingGroupByCode.Id != request.Id)
            throw new GroupExistingException(request.Code);
        
        var existingGroupByName = await repository.SingleOrDefaultAsync(new GroupByNameSpec(request.Name), cancellationToken);
        if (existingGroupByName != null && existingGroupByName.Id != request.Id)
            throw new GroupExistingException(request.Name);
        
        ArgumentNullException.ThrowIfNull(request);
        var group = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = group ?? throw new GroupNotFoundException(request.Id);
        var updatedApp = group.Update(request.Application, request.Parent, request.Tag, request.Number,
            request.Code, request.Name, request.Amount, request.EmployeeId, request.EmployeeName,
            request.Description, request.Notes);
        await repository.UpdateAsync(updatedApp, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("app group updated {GroupId}", updatedApp.Id);
        return new GroupUpdateResponse(updatedApp.Id);
    }
}
