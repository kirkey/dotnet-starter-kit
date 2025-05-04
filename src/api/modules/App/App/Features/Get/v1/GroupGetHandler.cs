using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.App.Domain;
using FSH.Starter.WebApi.App.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.App.Features.Get.v1;

public sealed class GroupGetHandler(
    [FromKeyedServices("Group")] IReadRepository<Group> repository,
    ICacheService cache)
    : IRequestHandler<GetAppRequest, GroupGetResponse>
{
    public async Task<GroupGetResponse> Handle(GetAppRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"Group:{request.Id}",
            async () =>
            {
                var group = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (group == null) throw new GroupNotFoundException(request.Id);
                return new GroupGetResponse(group.Id, group.Application, group.Parent, group.Tag,
                    group.Number, group.Code, group.Name, group.Amount,
                    group.EmployeeId, group.EmployeeName,
                    group.Description, group.Notes);
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
