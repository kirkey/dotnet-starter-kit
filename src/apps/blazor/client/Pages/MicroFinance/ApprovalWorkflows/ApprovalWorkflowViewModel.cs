using FSH.Starter.Blazor.Infrastructure.Api;
using Mapster;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ApprovalWorkflows;

public class ApprovalWorkflowViewModel
{
    public DefaultIdType Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public int NumberOfLevels { get; set; }
    public string? Description { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public DefaultIdType? BranchId { get; set; }
    public string? BranchName { get; set; }
    public bool IsSequential { get; set; } = true;
    public int Priority { get; set; } = 100;
    public bool IsActive { get; set; }

    public class Mapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ApprovalWorkflowResponse, ApprovalWorkflowViewModel>();
            config.NewConfig<ApprovalWorkflowViewModel, CreateApprovalWorkflowCommand>();
        }
    }
}
