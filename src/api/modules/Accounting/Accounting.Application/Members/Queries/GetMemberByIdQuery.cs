using Accounting.Application.Members.Responses;

namespace Accounting.Application.Members.Queries;

public class GetMemberByIdQuery : IRequest<MemberResponse>
{
    public DefaultIdType Id { get; set; }
}
