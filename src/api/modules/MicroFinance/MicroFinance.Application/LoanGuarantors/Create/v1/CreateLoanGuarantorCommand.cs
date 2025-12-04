using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Create.v1;

public sealed record CreateLoanGuarantorCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid LoanId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid GuarantorMemberId,
    [property: DefaultValue(50000)] decimal GuaranteedAmount,
    [property: DefaultValue("Relative")] string? Relationship,
    [property: DefaultValue("2025-01-15")] DateOnly? GuaranteeDate,
    [property: DefaultValue("2026-01-15")] DateOnly? ExpiryDate,
    [property: DefaultValue("Guarantor approved by committee")] string? Notes) : IRequest<CreateLoanGuarantorResponse>;
