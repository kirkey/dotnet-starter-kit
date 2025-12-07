using FSH.Starter.Blazor.Infrastructure.Api;
using Mapster;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CustomerCases;

public class CustomerCaseViewModel
{
    public DefaultIdType Id { get; set; }
    public string CaseNumber { get; set; } = string.Empty;
    public Guid MemberId { get; set; }
    public string? MemberName { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public string Priority { get; set; } = "Medium";
    public int SlaHours { get; set; } = 24;
    public string? Status { get; set; }
    public Guid? AssignedToId { get; set; }
    public string? AssignedToName { get; set; }
    public string? Resolution { get; set; }
    public int? SatisfactionScore { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public DateTime? ClosedAt { get; set; }

    public class Mapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CustomerCaseResponse, CustomerCaseViewModel>();
            config.NewConfig<CustomerCaseViewModel, CreateCustomerCaseCommand>();
        }
    }
}
