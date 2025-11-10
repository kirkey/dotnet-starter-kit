namespace Accounting.Application.Members.Activate.v1;

/// <summary>
/// Handler for activating a member account.
/// </summary>
public sealed class ActivateMemberHandler(
    [FromKeyedServices("accounting:members")] IRepository<Member> repository,
    ILogger<ActivateMemberHandler> logger)
    : IRequestHandler<ActivateMemberCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ActivateMemberCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        logger.LogInformation("Activating member {Id}", request.Id);

        var member = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (member == null)
            throw new NotFoundException($"Member with ID {request.Id} not found");

        member.Activate();
        
        await repository.UpdateAsync(member, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Member {MemberNumber} activated successfully", member.MemberNumber);
        return member.Id;
    }
}

