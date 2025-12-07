using Accounting.Application.Members.Responses;

namespace Accounting.Application.Members.Get.v1;

/// <summary>
/// Handler for retrieving a utility member by ID.
/// </summary>
public sealed class GetUtilityMemberHandler(
    [FromKeyedServices("accounting:members")] IReadRepository<Member> repository)
    : IRequestHandler<GetUtilityMemberRequest, UtilityMemberResponse>
{
    public async Task<UtilityMemberResponse> Handle(GetUtilityMemberRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new GetUtilityMemberByIdSpec(request.Id);
        var member = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false);

        if (member is null)
            throw new NotFoundException($"Member with ID {request.Id} was not found.");

        return member;
    }
}

