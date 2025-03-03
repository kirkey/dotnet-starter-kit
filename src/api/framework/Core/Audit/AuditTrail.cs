using System.ComponentModel.DataAnnotations.Schema;

namespace FSH.Framework.Core.Audit;
public class AuditTrail
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType UserId { get; set; }
    [Column(TypeName = "VARCHAR(1024)")]
    public string? UserName { get; set; }
    [Column(TypeName = "VARCHAR(64)")]
    public string PrimaryKey { get; set; } = string.Empty;
    public string? Operation { get; set; }
    public string? Entity { get; set; }
    public DateTimeOffset DateTime { get; set; }
    public string? PreviousValues { get; set; }
    public string? NewValues { get; set; }
    public string? ModifiedProperties { get; set; }
}
