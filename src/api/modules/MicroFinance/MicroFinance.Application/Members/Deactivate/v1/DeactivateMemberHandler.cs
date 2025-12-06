using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Deactivate.v1;

/// <summary>
/// Handler to deactivate an active member.
/// </summary>
public sealed class DeactivateMemberHandler(
    ILogger<DeactivateMemberHandler> logger,
    [FromKeyedServices("microfinance:members")] IRepository<Member> repository)
    : IRequestHandler<DeactivateMemberCommand, DeactivateMemberResponse>
{
    public async Task<DeactivateMemberResponse> Handle(DeactivateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await repository.FirstOrDefaultAsync(new MemberByIdSpec(request.Id), cancellationToken).ConfigureAwait(false)
            ?? throw new Exception($"Member {request.Id} not found");

        member.Deactivate();
        await repository.UpdateAsync(member, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Member {Id} ({MemberNumber}) deactivated", member.Id, member.MemberNumber);

        return new DeactivateMemberResponse(member.Id, member.IsActive);
    }
}
