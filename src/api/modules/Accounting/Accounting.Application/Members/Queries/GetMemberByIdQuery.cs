using Accounting.Application.Members.Dtos;

namespace Accounting.Application.Members.Queries;

public class GetMemberByIdQuery : IRequest<MemberDto>
{
    public DefaultIdType Id { get; set; }
}