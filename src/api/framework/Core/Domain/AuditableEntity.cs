using System.ComponentModel.DataAnnotations.Schema;
using FSH.Framework.Core.Domain.Contracts;

namespace FSH.Framework.Core.Domain;

public abstract class AuditableEntity : AuditableEntity<DefaultIdType>
{
    protected AuditableEntity() => Id = DefaultIdType.NewGuid();
}

public class AuditableEntity<TId> : BaseEntity<TId>, IAuditable, ISoftDeletable
{
    [Column(TypeName = "VARCHAR(1024)")]
    public string Name { get; set; } = string.Empty;
    [Column(TypeName = "VARCHAR(32)")]
    public string? Remarks { get; set; }
    [Column(TypeName = "VARCHAR(32)")]
    public string? Status { get; set; }
    [Column(TypeName = "VARCHAR(2048)")]
    public string? Description { get; set; }
    [Column(TypeName = "VARCHAR(2048)")]
    public string? Notes { get; set; }
    public string? FilePath { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DefaultIdType CreatedBy { get; set; }
    [Column(TypeName = "VARCHAR(64)")]
    public string? CreatedByUserName { get; set; }
    public DateTimeOffset LastModifiedOn { get; set; }
    public DefaultIdType? LastModifiedBy { get; set; }
    [Column(TypeName = "VARCHAR(64)")]
    public string? LastModifiedByUserName { get; set; }
    public DateTimeOffset? DeletedOn { get; set; }
    public DefaultIdType? DeletedBy { get; set; }
    [Column(TypeName = "VARCHAR(64)")]
    public string? DeletedByUserName { get; set; }
}

public abstract class AuditableEntityWithApproval : AuditableEntityWithApproval<DefaultIdType>;

public abstract class AuditableEntityWithApproval<TId> : AuditableEntity<TId>
{
    [Column(TypeName = "VARCHAR(1024)")]
    public string? Request { get; set; }
    [Column(TypeName = "VARCHAR(1024)")]
    public string? Feedback { get; set; }

    public DefaultIdType? PreparedBy { get; set; }
    [Column(TypeName = "VARCHAR(1024)")]
    public string? PreparerName { get; set; }
    public DateTimeOffset? PreparedOn { get; set; }

    public DefaultIdType? ReviewedBy { get; set; }
    [Column(TypeName = "VARCHAR(1024)")]
    public string? ReviewerName { get; set; }
    public DateTimeOffset? ReviewedOn { get; set; }

    public DefaultIdType? RecommendedBy { get; set; }
    [Column(TypeName = "VARCHAR(1024)")]
    public string? RecommenderName { get; set; }
    public DateTimeOffset? RecommendedOn { get; set; }

    public DefaultIdType? ApprovedBy { get; set; }
    [Column(TypeName = "VARCHAR(1024)")]
    public string? ApproverName { get; set; }
    public DateTimeOffset? ApprovedOn { get; set; }
}
