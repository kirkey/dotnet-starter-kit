namespace FSH.Framework.Core.Extensions.Dto;

public record BaseDto<TId>
{
    public TId Id { get; protected init; } = default!;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public string? Remarks { get; set; }
    public string? Status { get; set; }
    public string? FilePath { get; set; }
    public DateTimeOffset CreatedOn { get; protected init; }
    public DefaultIdType CreatedBy { get; protected init; }
    public string? CreatedByUserName { get; protected init; }
    public DateTimeOffset LastModifiedOn { get; set; }
    public DefaultIdType? LastModifiedBy { get; set; }
    public string? LastModifiedByUserName { get; set; }
}

public abstract record BaseDto : BaseDto<DefaultIdType>;
