using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Update.v1;

/// <summary>
/// Command to update an existing member.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Updates the profile information of an existing member. Only provided (non-null)
/// fields are updated; omitted fields remain unchanged.
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate member exists</description></item>
///   <item><description>Apply only provided (non-null) updates</description></item>
///   <item><description>Validate updated fields against rules</description></item>
///   <item><description>Update member record</description></item>
///   <item><description>Raise MemberUpdated domain event if changes made</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>Id: Required, must be an existing member</description></item>
///   <item><description>FirstName: If provided, 2-128 characters</description></item>
///   <item><description>LastName: If provided, 2-128 characters</description></item>
///   <item><description>Email: If provided, valid email format, max 256 characters</description></item>
///   <item><description>MonthlyIncome: If provided, must be non-negative</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Update</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
///   "email": "john.doe.updated@example.com",
///   "phoneNumber": "+1-555-987-6543",
///   "address": "456 Oak Avenue, Suite 100",
///   "occupation": "Restaurant Owner",
///   "monthlyIncome": 7500.00
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="UpdateMemberResponse"/> with updated member details.</para>
/// </remarks>
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
    string? NationalId = null,
    string? Occupation = null,
    decimal? MonthlyIncome = null,
    string? Notes = null)
    : IRequest<UpdateMemberResponse>;
