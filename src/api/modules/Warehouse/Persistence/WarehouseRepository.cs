using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Framework.Core.Persistence;
using Mapster;

namespace FSH.Starter.WebApi.Warehouse.Persistence;

internal sealed class WarehouseRepository<T>(WarehouseDbContext context) : RepositoryBase<T>(context), IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) =>
        specification.Selector is not null
            ? base.ApplySpecification(specification)
            : ApplySpecification(specification, false)
                .ProjectToType<TResult>();
}

