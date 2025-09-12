// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/Store.Infrastructure/Persistence/StoreRepository.cs
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using FSH.Framework.Core.Domain.Contracts;
using Mapster;

namespace Store.Infrastructure.Persistence;

internal sealed class StoreRepository<T>(StoreDbContext context) : RepositoryBase<T>(context), IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    // Override to project results with Mapster when no selector is provided
    protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) =>
        specification.Selector is not null
            ? base.ApplySpecification(specification)
            : ApplySpecification(specification, false)
                .ProjectToType<TResult>();
}

