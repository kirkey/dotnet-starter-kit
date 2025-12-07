using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Infrastructure.Api;
using Mapster;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ApprovalRequests;

public class ApprovalRequestViewModel : IEntity<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public string RequestNumber { get; set; } = string.Empty;
    public DefaultIdType WorkflowId { get; set; }
    public string? WorkflowName { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public DefaultIdType EntityId { get; set; }
    public int TotalLevels { get; set; }
    public int CurrentLevel { get; set; }
    public DefaultIdType SubmittedById { get; set; }
    public string? SubmittedByName { get; set; }
    public decimal? Amount { get; set; }
    public DefaultIdType? BranchId { get; set; }
    public string? BranchName { get; set; }
    public string? Comments { get; set; }
    public int? SlaHours { get; set; }
    public string? Status { get; set; }
    public DateTimeOffset? SubmittedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }

    public class Mapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ApprovalRequestResponse, ApprovalRequestViewModel>();
            config.NewConfig<ApprovalRequestViewModel, CreateApprovalRequestCommand>();
        }
    }
}
