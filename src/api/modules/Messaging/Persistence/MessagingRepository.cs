using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using FSH.Framework.Core.Domain.Contracts;
using Mapster;

namespace FSH.Starter.WebApi.Messaging.Persistence;

/// <summary>
/// Repository implementation for the Messaging module entities.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
internal sealed class MessagingRepository<T>(MessagingDbContext context) : RepositoryBase<T>(context), IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    // We override the default behavior when mapping to a dto.
    // We're using Mapster's ProjectToType here to immediately map the result from the database.
    // This is only done when no Selector is defined, so regular specifications with a selector also still work.
    protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) =>
        specification.Selector is not null
            ? base.ApplySpecification(specification)
            : ApplySpecification(specification, false)
                .ProjectToType<TResult>();
}

