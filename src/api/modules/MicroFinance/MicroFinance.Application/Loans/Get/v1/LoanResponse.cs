namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Get.v1;

public sealed record LoanResponse(
    Guid Id,
    Guid MemberId,
    string MemberName,
    string MemberNumber,
    Guid LoanProductId,
    string LoanProductName,
    string LoanNumber,
    decimal PrincipalAmount,
    decimal InterestRate,
    int TermMonths,
    string RepaymentFrequency,
    string? Purpose,
    DateOnly ApplicationDate,
    DateOnly? ApprovalDate,
    DateOnly? DisbursementDate,
    DateOnly? ExpectedEndDate,
    DateOnly? ActualEndDate,
    decimal OutstandingPrincipal,
    decimal OutstandingInterest,
    decimal TotalPaid,
    string Status,
    string? RejectionReason,
    IReadOnlyList<LoanScheduleDto> Schedules,
    IReadOnlyList<LoanRepaymentDto> Repayments,
    IReadOnlyList<LoanGuarantorDto> Guarantors,
    IReadOnlyList<LoanCollateralDto> Collaterals
);

public sealed record LoanScheduleDto(
    Guid Id,
    int InstallmentNumber,
    DateOnly DueDate,
    decimal PrincipalAmount,
    decimal InterestAmount,
    decimal TotalAmount,
    decimal PaidAmount,
    bool IsPaid,
    DateOnly? PaidDate
);

public sealed record LoanRepaymentDto(
    Guid Id,
    DateOnly RepaymentDate,
    decimal TotalAmount,
    decimal PrincipalAmount,
    decimal InterestAmount,
    decimal PenaltyAmount,
    string PaymentMethod,
    string? Notes
);

public sealed record LoanGuarantorDto(
    Guid Id,
    Guid GuarantorMemberId,
    string? GuarantorName,
    string? Relationship,
    decimal GuaranteedAmount,
    string Status
);

public sealed record LoanCollateralDto(
    Guid Id,
    string CollateralType,
    string Description,
    decimal EstimatedValue,
    string? DocumentReference,
    string Status
);
