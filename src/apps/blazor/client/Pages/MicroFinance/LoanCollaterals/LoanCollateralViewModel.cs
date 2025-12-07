using FSH.Starter.Blazor.Infrastructure.Api;
using Mapster;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanCollaterals;

public class LoanCollateralViewModel
{
    public DefaultIdType Id { get; set; }
    public Guid LoanId { get; set; }
    public string? LoanNumber { get; set; }
    public string CollateralType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal EstimatedValue { get; set; }
    public decimal? ForcedSaleValue { get; set; }
    public DateOnly? ValuationDate { get; set; }
    public string? Location { get; set; }
    public string? DocumentReference { get; set; }
    public string? Notes { get; set; }
    public string? Status { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public DateTime? PledgedAt { get; set; }
    public DateTime? ReleasedAt { get; set; }
    public DateTime? SeizedAt { get; set; }

    public class Mapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<LoanCollateralResponse, LoanCollateralViewModel>();
            config.NewConfig<LoanCollateralViewModel, CreateLoanCollateralCommand>();
        }
    }
}
