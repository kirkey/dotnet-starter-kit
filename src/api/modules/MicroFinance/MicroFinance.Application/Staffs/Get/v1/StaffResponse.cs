namespace FSH.Starter.WebApi.MicroFinance.Application.Staffs.Get.v1;

public sealed record StaffResponse(
    DefaultIdType Id,
    string EmployeeNumber,
    string FirstName,
    string LastName,
    string? MiddleName,
    string FullName,
    string Email,
    string? Phone,
    string JobTitle,
    string Role,
    string EmploymentType,
    string Status,
    DefaultIdType? BranchId,
    string? Department,
    DateOnly JoiningDate,
    DateOnly? ConfirmationDate,
    DefaultIdType? ReportingManagerId,
    string? ReportingTo,
    bool CanApproveLoan,
    decimal? LoanApprovalLimit);
