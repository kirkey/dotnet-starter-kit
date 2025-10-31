using Accounting.Application.Banks.Get.v1;

namespace Accounting.Application.Banks.Search.v1;

/// <summary>
/// Handler for searching banks with filtering and pagination.
/// </summary>
public sealed class BankSearchHandler(
    [FromKeyedServices("accounting:banks")] IReadRepository<Bank> repository)
    : IRequestHandler<BankSearchCommand, PagedList<BankResponse>>
{
    /// <summary>
    /// Handles the search operation for banks.
    /// </summary>
    /// <param name="request">The search command containing filter criteria.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>Paged list of banks matching the search criteria.</returns>
    public async Task<PagedList<BankResponse>> Handle(BankSearchCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new BankSearchSpecs(request);

        var banks = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var bankResponses = banks.Select(bank => new BankResponse(
            bank.Id,
            bank.BankCode,
            bank.Name,
            bank.RoutingNumber,
            bank.SwiftCode,
            bank.Address,
            bank.ContactPerson,
            bank.PhoneNumber,
            bank.Email,
            bank.Website,
            bank.Description,
            bank.Notes,
            bank.IsActive,
            bank.ImageUrl)).ToList();

        return new PagedList<BankResponse>(bankResponses, totalCount, request.PageNumber, request.PageSize);
    }
}

