namespace Accounting.Application.Members.Create.v1;

/// <summary>
/// Handler for creating a new utility member account.
/// </summary>
public sealed class CreateUtilityMemberHandler(
    [FromKeyedServices("accounting:members")] IRepository<Member> repository,
    ILogger<CreateUtilityMemberHandler> logger)
    : IRequestHandler<CreateUtilityMemberCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateUtilityMemberCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Creating member {MemberNumber}", request.MemberNumber);

        // Check for duplicate member number
        var spec = new Specification<Member>();
        spec.Query.Where(m => m.MemberNumber == request.MemberNumber);
        var existing = await repository.FirstOrDefaultAsync(spec, cancellationToken);
        
        if (existing != null)
            throw new InvalidOperationException($"Member number '{request.MemberNumber}' already exists.");

        var member = Member.Create(
            request.MemberNumber,
            request.MemberName,
            request.ServiceAddress,
            request.MembershipDate,
            request.MailingAddress,
            request.ContactInfo,
            request.AccountStatus,
            request.MeterId,
            request.Email,
            request.PhoneNumber,
            request.EmergencyContact,
            request.ServiceClass,
            request.RateScheduleId,
            request.RateSchedule,
            request.Description,
            request.Notes);

        await repository.AddAsync(member, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Member {MemberNumber} created with ID {MemberId}", 
            member.MemberNumber, member.Id);
        
        return member.Id;
    }
}

