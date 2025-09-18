using Accounting.Application.Member.Dtos;

namespace Accounting.Application.Member.Queries
{
    public class GetMemberByIdQuery : IRequest<MemberDto>
    {
        public DefaultIdType Id { get; set; }
    }
}

