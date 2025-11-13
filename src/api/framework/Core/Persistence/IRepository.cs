using Ardalis.Specification;

namespace FSH.Framework.Core.Persistence;
public interface IRepository<T> : IRepositoryBase<T>
    where T : class;

public interface IReadRepository<T> : IReadRepositoryBase<T>
    where T : class;
