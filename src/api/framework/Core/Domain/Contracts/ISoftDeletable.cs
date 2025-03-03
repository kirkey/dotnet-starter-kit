namespace FSH.Framework.Core.Domain.Contracts;

public interface ISoftDeletable
{
    DateTimeOffset? DeletedOn { get; set; }
    DefaultIdType? DeletedBy { get; set; }
    string? DeletedByUserName { get; set; }
}
