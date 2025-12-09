using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Create.v1;

/// <summary>
/// Command to create a new member in the microfinance system.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Registers a new member (client) in the microfinance institution.
/// The member is created in active status and can immediately access financial services.
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate all required fields</description></item>
///   <item><description>Verify member number is unique</description></item>
///   <item><description>Validate email format if provided</description></item>
///   <item><description>Create member record with IsActive = true</description></item>
///   <item><description>Raise MemberCreated domain event</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>MemberNumber: Optional (auto-generated if not provided), max 64 characters, must be unique</description></item>
///   <item><description>FirstName: Required, 2-128 characters</description></item>
///   <item><description>LastName: Required, 2-128 characters</description></item>
///   <item><description>Email: Optional, max 256 characters, valid email format</description></item>
///   <item><description>PhoneNumber: Optional, max 32 characters</description></item>
///   <item><description>DateOfBirth: Optional, must be a date in the past</description></item>
///   <item><description>MonthlyIncome: Optional, must be non-negative if provided</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Create</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "memberNumber": "MBR-2024-001234",
///   "firstName": "John",
///   "lastName": "Doe",
///   "middleName": "Michael",
///   "email": "john.doe@example.com",
///   "phoneNumber": "+1-555-123-4567",
///   "dateOfBirth": "1985-06-15",
///   "gender": "Male",
///   "address": "123 Main Street, Apt 4B",
///   "city": "New York",
///   "state": "NY",
///   "postalCode": "10001",
///   "country": "USA",
///   "nationalId": "SSN-123-45-6789",
///   "occupation": "Small Business Owner",
///   "monthlyIncome": 5000.00,
///   "notes": "Referred by existing member"
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="CreateMemberResponse"/> containing the created member ID.</para>
/// </remarks>
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
