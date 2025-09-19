using Accounting.Application.Members.Responses;
using Accounting.Application.Members.Queries;

namespace Accounting.Application.Members.Handlers;

public class GetMemberByIdHandler(IReadRepository<Member> repository)
    : IRequestHandler<GetMemberByIdQuery, MemberResponse>
{
    public async Task<MemberResponse> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var member = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (member == null)
            throw new NotFoundException($"Member with Id {request.Id} not found");

        return new MemberResponse
        {
            Id = member.Id,
            MemberNumber = member.MemberNumber,
            MemberName = member.MemberName,
            ServiceAddress = member.ServiceAddress,
            MailingAddress = member.MailingAddress,
            ContactInfo = member.ContactInfo,
            AccountStatus = member.AccountStatus,
            MeterId = member.MeterId,
            RateScheduleId = member.RateScheduleId,
            MembershipDate = member.MembershipDate,
            CurrentBalance = member.CurrentBalance,
            IsActive = member.IsActive,
            Email = member.Email,
            PhoneNumber = member.PhoneNumber,
            EmergencyContact = member.EmergencyContact,
            ServiceClass = member.ServiceClass,
            RateSchedule = member.RateSchedule,
            Description = member.Description,
            Notes = member.Notes
        };
    }
}
