using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Update.v1;

/// <summary>
/// Command to update an existing member.
/// </summary>
public sealed record UpdateMemberCommand(
    DefaultIdType Id,
    string? FirstName = null,
    string? LastName = null,
    string? MiddleName = null,
    string? Email = null,
    string? PhoneNumber = null,
    DateOnly? DateOfBirth = null,
    string? Gender = null,
    string? Address = null,
    string? City = null,
    string? State = null,
    string? PostalCode = null,
    string? Country = null,
    string? NationalId = null,
    string? Occupation = null,
    decimal? MonthlyIncome = null,
    string? Notes = null)
    : IRequest<UpdateMemberResponse>;
