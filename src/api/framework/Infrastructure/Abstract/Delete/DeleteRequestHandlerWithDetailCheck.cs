using Ardalis.Specification;
using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;

namespace FSH.Framework.Infrastructure.Abstract.Delete;

public class DeleteRequestHandlerWithDetailCheck<TRequest, TEntity, TId, TCheck>(
    IRepository<TEntity> repository,
    IReadRepository<TCheck> checkRepository,
    Func<TRequest, ISpecification<TCheck>> checkSpecFactory,
    string? conflictMessage = null,
    ICacheService? cacheService = null)
    : DeleteRequestHandler<TRequest, TEntity, TId>(repository, cacheService)
    where TRequest : DeleteRequest<TId>
    where TEntity : class, IAggregateRoot
    where TId : notnull
    where TCheck : class, IAggregateRoot
{
    public override async Task<TId> Handle(TRequest request, CancellationToken cancellationToken)
    {
        return await checkRepository.AnyAsync(
                checkSpecFactory(request), cancellationToken).ConfigureAwait(false)
            ? throw new CustomException(
                conflictMessage ?? $"{typeof(TEntity).Name} {request.Id} cannot be deleted as it's being used.")
            : await base.Handle(request, cancellationToken);
    }
}
