using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Delete.v1;

/// <summary>
/// Handler for deleting a member.
/// </summary>
public sealed class DeleteMemberHandler(
    ILogger<DeleteMemberHandler> logger,
    [FromKeyedServices("microfinance:members")] IRepository<Member> repository)
    : IRequestHandler<DeleteMemberCommand>
{
    /// <summary>
    /// Handles the DeleteMemberCommand request.
    /// </summary>
    /// <param name="request">The delete member command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var member = await repository.FirstOrDefaultAsync(
            new MemberByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (member is null)
        {
            throw new NotFoundException($"Member with ID {request.Id} not found.");
        }

        await repository.DeleteAsync(member, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Member deleted with ID: {MemberId}, Name: {Name}", member.Id, member.FullName);
    }
}
