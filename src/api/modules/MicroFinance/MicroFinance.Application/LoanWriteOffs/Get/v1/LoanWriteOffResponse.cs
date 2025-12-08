namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Get.v1;

public sealed record LoanWriteOffResponse(
    DefaultIdType Id,
    DefaultIdType LoanId,
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
    DefaultIdType? ApprovedByUserId,
    string? ApprovedBy,
    DateTime? ApprovedAt);
