using Accounting.Application.Members.Responses;

namespace Accounting.Application.Members.Get.v1;

/// <summary>
/// Handler for retrieving a member by ID.
/// </summary>
public sealed class GetMemberHandler(
    [FromKeyedServices("accounting:members")] IReadRepository<Member> repository)
    : IRequestHandler<GetMemberRequest, MemberResponse>
{
    public async Task<MemberResponse> Handle(GetMemberRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new GetMemberByIdSpec(request.Id);
        var member = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false);

        if (member is null)
            throw new NotFoundException($"Member with ID {request.Id} was not found.");

        return member;
    }
}

