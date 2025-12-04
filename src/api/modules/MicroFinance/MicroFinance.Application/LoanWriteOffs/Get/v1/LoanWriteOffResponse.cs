namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Get.v1;

public sealed record LoanWriteOffResponse(
    Guid Id,
    Guid LoanId,
    string WriteOffNumber,
    string WriteOffType,
    string Reason,
    DateOnly RequestDate,
    DateOnly? WriteOffDate,
    decimal PrincipalWriteOff,
    decimal InterestWriteOff,
    decimal PenaltiesWriteOff,
    decimal FeesWriteOff,
    decimal TotalWriteOff,
    decimal RecoveredAmount,
    int DaysPastDue,
    int CollectionAttempts,
    string Status,
    Guid? ApprovedByUserId,
    string? ApprovedBy,
    DateTime? ApprovedAt);
