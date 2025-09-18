using Accounting.Application.Member.Queries;
using Accounting.Application.Member.Dtos;

namespace Accounting.Application.Member.Handlers
{
    public class GetMemberByIdHandler(IReadRepository<Accounting.Domain.Member> repository)
        : IRequestHandler<GetMemberByIdQuery, MemberDto>
    {
        public async Task<MemberDto> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
        {
            var member = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (member == null)
                throw new NotFoundException($"Member with Id {request.Id} not found");

            return new MemberDto
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
}

