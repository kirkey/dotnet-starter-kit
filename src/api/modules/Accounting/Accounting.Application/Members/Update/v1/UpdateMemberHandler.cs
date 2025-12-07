namespace Accounting.Application.Members.Update.v1;

/// <summary>
/// Handler for updating a member account.
/// </summary>
public sealed class UpdateUtilityMemberHandler(
    [FromKeyedServices("accounting:members")] IRepository<Member> repository,
    ILogger<UpdateUtilityMemberHandler> logger)
    : IRequestHandler<UpdateUtilityMemberCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateUtilityMemberCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        logger.LogInformation("Updating member {Id}", request.Id);

        var member = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (member == null)
            throw new NotFoundException($"Member with ID {request.Id} not found");

        member.Update(
            request.MemberName,
            request.ServiceAddress,
            request.MailingAddress,
            request.ContactInfo,
            request.AccountStatus,
            request.MeterId,
            request.Email,
            request.PhoneNumber,
            request.EmergencyContact,
            request.ServiceClass,
            request.RateSchedule,
            request.Description,
            request.Notes);

        await repository.UpdateAsync(member, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Member {MemberNumber} updated successfully", member.MemberNumber);
        return member.Id;
    }
}

