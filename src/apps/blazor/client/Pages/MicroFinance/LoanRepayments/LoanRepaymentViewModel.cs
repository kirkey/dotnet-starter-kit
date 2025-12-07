using FSH.Starter.Blazor.Infrastructure.Api;
using Mapster;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanRepayments;

public class LoanRepaymentViewModel
{
    public DefaultIdType Id { get; set; }
    public Guid LoanId { get; set; }
    public string? LoanNumber { get; set; }
    public string? MemberName { get; set; }
    public decimal Amount { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestAmount { get; set; }
    public decimal PenaltyAmount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? TransactionReference { get; set; }
    public DateOnly PaymentDate { get; set; }
    public string? ReceiptNumber { get; set; }
    public string? Notes { get; set; }
    public bool IsReversed { get; set; }

    public class Mapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<LoanRepaymentResponse, LoanRepaymentViewModel>();
            config.NewConfig<LoanRepaymentViewModel, CreateLoanRepaymentCommand>();
        }
    }
}
