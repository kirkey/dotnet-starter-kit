using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Update.v1;

/// <summary>
/// Handler for updating a member.
/// </summary>
public sealed class UpdateMemberHandler(
    ILogger<UpdateMemberHandler> logger,
    [FromKeyedServices("microfinance:members")] IRepository<Member> repository)
    : IRequestHandler<UpdateMemberCommand, UpdateMemberResponse>
{
    /// <summary>
    /// Handles the UpdateMemberCommand request.
    /// </summary>
    /// <param name="request">The update member command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A response containing the updated member ID.</returns>
    public async Task<UpdateMemberResponse> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var member = await repository.FirstOrDefaultAsync(
            new MemberByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (member is null)
        {
            throw new NotFoundException($"Member with ID {request.Id} not found.");
        }

        member.Update(
            firstName: request.FirstName,
            lastName: request.LastName,
            middleName: request.MiddleName,
            email: request.Email,
            phoneNumber: request.PhoneNumber,
            dateOfBirth: request.DateOfBirth,
            gender: request.Gender,
            address: request.Address,
            nationalId: request.NationalId,
            occupation: request.Occupation,
            monthlyIncome: request.MonthlyIncome,
            notes: request.Notes);

        await repository.UpdateAsync(member, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Member updated with ID: {MemberId}, Name: {Name}", member.Id, member.FullName);

        return new UpdateMemberResponse(member.Id);
    }
}
