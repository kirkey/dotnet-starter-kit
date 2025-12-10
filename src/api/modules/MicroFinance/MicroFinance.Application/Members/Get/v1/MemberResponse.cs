namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Get.v1;

/// <summary>
/// Response containing member details.
/// </summary>
public sealed record MemberResponse(
    DefaultIdType Id,
    string MemberNumber,
    string FirstName,
    string LastName,
    string? MiddleName,
    string FullName,
    string? Email,
    string? PhoneNumber,
    DateOnly? DateOfBirth,
    string? Gender,
    string? Address,
    string? NationalId,
    string? Occupation,
    decimal? MonthlyIncome,
    DateOnly JoinDate,
    bool IsActive,
    string? Notes);
