using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Create.v1;

public sealed record CreateLoanCollateralCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid LoanId,
    [property: DefaultValue("RealEstate")] string CollateralType,
    [property: DefaultValue("2-bedroom house with land title")] string Description,
    [property: DefaultValue(500000)] decimal EstimatedValue,
    [property: DefaultValue(350000)] decimal? ForcedSaleValue,
    [property: DefaultValue("2025-01-15")] DateOnly? ValuationDate,
    [property: DefaultValue("123 Main Street, Manila")] string? Location,
    [property: DefaultValue("TCT-12345")] string? DocumentReference,
    [property: DefaultValue("Property verified by field officer")] string? Notes) : IRequest<CreateLoanCollateralResponse>;
