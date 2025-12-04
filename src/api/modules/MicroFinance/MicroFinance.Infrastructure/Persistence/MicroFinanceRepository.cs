using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using FSH.Framework.Core.Domain.Contracts;
using Mapster;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence;

/// <summary>
/// Generic repository implementation for MicroFinance entities.
/// Provides CRUD operations with specification pattern support.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
internal sealed class MicroFinanceRepository<T>(MicroFinanceDbContext context)
    : RepositoryBase<T>(context), IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    /// <inheritdoc/>
    protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) =>
        specification.Selector is not null
            ? base.ApplySpecification(specification)
            : ApplySpecification(specification, false)
                .ProjectToType<TResult>();
}

