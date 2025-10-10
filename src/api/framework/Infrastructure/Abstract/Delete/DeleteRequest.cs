namespace FSH.Framework.Infrastructure.Abstract.Delete;

public abstract class DeleteRequest<TId>(TId id) : IRequest<TId>
{
    public TId Id { get; } = id;
}
