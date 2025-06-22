using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.App.Domain;
using FSH.Starter.WebApi.App.Features.Dtos;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.App.Features.Search.v1;
public sealed class GroupSearchHandler(
    [FromKeyedServices("app:group")] IReadRepository<Group> repository)
    : IRequestHandler<GroupSearchCommand, PagedList<GroupDto>>
{
    public async Task<PagedList<GroupDto>> Handle(GroupSearchCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var spec = new GroupSearchSpec(command);

        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<GroupDto>(list, command.PageNumber, command.PageSize, totalCount);
    }
}
