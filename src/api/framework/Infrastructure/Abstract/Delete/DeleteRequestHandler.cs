using Ardalis.Specification;
using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Framework.Core.Exceptions;
using MediatR;

namespace FSH.Framework.Infrastructure.Abstract.Delete;

public abstract class DeleteRequestHandler<TRequest, TEntity, TId>(
    IRepositoryBase<TEntity> repository,
    ICacheService? cacheService) : IRequestHandler<TRequest, TId>
    where TRequest : DeleteRequest<TId>
    where TEntity : class, IAggregateRoot
    where TId : notnull
{
    public virtual async Task<TId> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = entity ?? throw new NotFoundException($"{typeof(TEntity).Name} {request.Id} not found.");

        await repository.DeleteAsync(entity, cancellationToken).ConfigureAwait(false);

        if (cacheService == null)
        {
            return request.Id;
        }

        await cacheService.RemoveAsync($"{typeof(TEntity).Name}:{request.Id}", cancellationToken).ConfigureAwait(false);
        await cacheService.RemoveAsync($"{typeof(TEntity).Name}Dto:{request.Id}", cancellationToken).ConfigureAwait(false);

        return request.Id;
    }
}
