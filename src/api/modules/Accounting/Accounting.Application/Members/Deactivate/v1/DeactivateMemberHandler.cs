namespace Accounting.Application.Members.Deactivate.v1;

/// <summary>
/// Handler for deactivating a utility member account.
/// </summary>
public sealed class DeactivateUtilityMemberHandler(
    [FromKeyedServices("accounting:members")] IRepository<Member> repository,
    ILogger<DeactivateUtilityMemberHandler> logger)
    : IRequestHandler<DeactivateUtilityMemberCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(DeactivateUtilityMemberCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        logger.LogInformation("Deactivating member {Id}", request.Id);

        var member = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (member == null)
            throw new NotFoundException($"Member with ID {request.Id} not found");

        member.Deactivate();
        
        await repository.UpdateAsync(member, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Member {MemberNumber} deactivated successfully", member.MemberNumber);
        return member.Id;
    }
}

