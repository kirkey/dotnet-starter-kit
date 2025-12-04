using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Create.v1;

/// <summary>
/// Handler for creating a new member.
/// </summary>
public sealed class CreateMemberHandler(
    ILogger<CreateMemberHandler> logger,
    [FromKeyedServices("microfinance:members")] IRepository<Member> repository)
    : IRequestHandler<CreateMemberCommand, CreateMemberResponse>
{
    /// <summary>
    /// Handles the CreateMemberCommand request.
    /// </summary>
    /// <param name="request">The create member command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A response containing the newly created member ID.</returns>
    public async Task<CreateMemberResponse> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var member = Member.Create(
            memberNumber: request.MemberNumber!,
            firstName: request.FirstName!,
            lastName: request.LastName!,
            middleName: request.MiddleName,
            email: request.Email,
            phoneNumber: request.PhoneNumber,
            dateOfBirth: request.DateOfBirth,
            gender: request.Gender,
            address: request.Address,
            city: request.City,
            state: request.State,
            postalCode: request.PostalCode,
            country: request.Country,
            nationalId: request.NationalId,
            occupation: request.Occupation,
            monthlyIncome: request.MonthlyIncome,
            joinDate: request.JoinDate,
            notes: request.Notes);

        await repository.AddAsync(member, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Member created with ID: {MemberId}, MemberNumber: {MemberNumber}, Name: {Name}",
            member.Id, member.MemberNumber, member.FullName);

        return new CreateMemberResponse(member.Id);
    }
}
