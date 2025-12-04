namespace FSH.Starter.WebApi.MicroFinance.Application.Staff.Get.v1;

public sealed record StaffResponse(
    Guid Id,
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
    Guid? BranchId,
    string? Department,
    DateOnly JoiningDate,
    DateOnly? ConfirmationDate,
    Guid? ReportingManagerId,
    string? ReportingTo,
    bool CanApproveLoan,
    decimal? LoanApprovalLimit);
