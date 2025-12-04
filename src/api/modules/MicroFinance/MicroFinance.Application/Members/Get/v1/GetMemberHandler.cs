using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Get.v1;

/// <summary>
/// Handler for getting a member by ID.
/// </summary>
public sealed class GetMemberHandler(
    [FromKeyedServices("microfinance:members")] IReadRepository<Member> repository)
    : IRequestHandler<GetMemberRequest, MemberResponse>
{
    /// <summary>
    /// Handles the GetMemberRequest.
    /// </summary>
    /// <param name="request">The get member request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The member response.</returns>
    public async Task<MemberResponse> Handle(GetMemberRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var member = await repository.FirstOrDefaultAsync(
            new MemberByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (member is null)
        {
            throw new NotFoundException($"Member with ID {request.Id} not found.");
        }

        return new MemberResponse(
            member.Id,
            member.MemberNumber,
            member.FirstName,
            member.LastName,
            member.MiddleName,
            member.FullName,
            member.Email,
            member.PhoneNumber,
            member.DateOfBirth,
            member.Gender,
            member.Address,
            member.City,
            member.State,
            member.PostalCode,
            member.Country,
            member.NationalId,
            member.Occupation,
            member.MonthlyIncome,
            member.JoinDate,
            member.IsActive,
            member.Notes);
    }
}
