namespace Accounting.Application.Members.Delete.v1;

/// <summary>
/// Handler for deleting a member account.
/// Only inactive members with no balances or transaction history can be deleted.
/// </summary>
public sealed class DeleteUtilityMemberHandler(
    [FromKeyedServices("accounting:members")] IRepository<Member> repository,
    ILogger<DeleteUtilityMemberHandler> logger)
    : IRequestHandler<DeleteUtilityMemberCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(DeleteUtilityMemberCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        logger.LogInformation("Deleting member {Id}", request.Id);

        var member = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (member == null)
            throw new NotFoundException($"Member with ID {request.Id} not found");

        if (member.IsActive)
            throw new InvalidOperationException("Cannot delete an active member. Deactivate the member first.");

        if (member.CurrentBalance != 0)
            throw new InvalidOperationException($"Cannot delete member with non-zero balance ({member.CurrentBalance:C}).");

        await repository.DeleteAsync(member, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Member {MemberNumber} deleted successfully", member.MemberNumber);
        return request.Id;
    }
}

