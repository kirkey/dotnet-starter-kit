namespace Accounting.Application.Members.UpdateBalance.v1;

/// <summary>
/// Handler for updating a member's balance.
/// </summary>
public sealed class UpdateMemberBalanceHandler(
    [FromKeyedServices("accounting:members")] IRepository<Member> repository,
    ILogger<UpdateMemberBalanceHandler> logger)
    : IRequestHandler<UpdateMemberBalanceCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateMemberBalanceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        logger.LogInformation("Updating balance for member {Id} to {Balance}", request.Id, request.NewBalance);

        var member = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (member == null)
            throw new NotFoundException($"Member with ID {request.Id} not found");

        member.UpdateBalance(request.NewBalance);
        
        await repository.UpdateAsync(member, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Member {MemberNumber} balance updated to {Balance}", 
            member.MemberNumber, member.CurrentBalance);
        return member.Id;
    }
}

