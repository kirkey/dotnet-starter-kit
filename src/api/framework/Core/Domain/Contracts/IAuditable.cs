namespace FSH.Framework.Core.Domain.Contracts;

public interface IAuditable
{
    DateTimeOffset CreatedOn { get; }
    DefaultIdType CreatedBy { get; }
    string? CreatedByUserName { get; }
    DateTimeOffset LastModifiedOn { get; }
    DefaultIdType? LastModifiedBy { get; }
    string? LastModifiedByUserName { get; }
}
