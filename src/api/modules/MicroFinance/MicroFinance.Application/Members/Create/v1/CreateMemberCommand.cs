using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Create.v1;

/// <summary>
/// Command to create a new member in the microfinance system.
/// </summary>
public sealed record CreateMemberCommand(
    [property: DefaultValue("MBR-001")] string? MemberNumber,
    [property: DefaultValue("John")] string? FirstName,
    [property: DefaultValue("Doe")] string? LastName,
    [property: DefaultValue(null)] string? MiddleName = null,
    [property: DefaultValue("john.doe@example.com")] string? Email = null,
    [property: DefaultValue("+1234567890")] string? PhoneNumber = null,
    [property: DefaultValue(null)] DateOnly? DateOfBirth = null,
    [property: DefaultValue("Male")] string? Gender = null,
    [property: DefaultValue("123 Main Street")] string? Address = null,
    [property: DefaultValue("New York")] string? City = null,
    [property: DefaultValue("NY")] string? State = null,
    [property: DefaultValue("10001")] string? PostalCode = null,
    [property: DefaultValue("USA")] string? Country = null,
    [property: DefaultValue(null)] string? NationalId = null,
    [property: DefaultValue("Self-employed")] string? Occupation = null,
    [property: DefaultValue(5000)] decimal? MonthlyIncome = null,
    [property: DefaultValue(null)] DateOnly? JoinDate = null,
    [property: DefaultValue(null)] string? Notes = null)
    : IRequest<CreateMemberResponse>;
