using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Activate.v1;

/// <summary>
/// Handler to activate an inactive member.
/// </summary>
public sealed class ActivateMemberHandler(
    ILogger<ActivateMemberHandler> logger,
    [FromKeyedServices("microfinance:members")] IRepository<Member> repository)
    : IRequestHandler<ActivateMemberCommand, ActivateMemberResponse>
{
    public async Task<ActivateMemberResponse> Handle(ActivateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await repository.FirstOrDefaultAsync(new MemberByIdSpec(request.Id), cancellationToken).ConfigureAwait(false)
            ?? throw new NotFoundException($"Member {request.Id} not found");

        member.Activate();
        await repository.UpdateAsync(member, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Member {Id} ({MemberNumber}) activated", member.Id, member.MemberNumber);

        return new ActivateMemberResponse(member.Id, member.IsActive);
    }
}
