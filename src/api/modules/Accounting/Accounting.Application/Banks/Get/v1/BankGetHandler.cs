using Accounting.Application.Banks.Specs;
using Accounting.Domain.Entities;

namespace Accounting.Application.Banks.Get.v1;

/// <summary>
/// Handler for retrieving a bank by its unique identifier.
/// </summary>
public sealed class BankGetHandler(
    [FromKeyedServices("accounting:banks")] IReadRepository<Bank> repository)
    : IRequestHandler<BankGetRequest, BankResponse>
{
    /// <summary>
    /// Handles the retrieval of a bank entity.
    /// </summary>
    /// <param name="request">The get bank request containing the bank ID.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>Response containing the bank details.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    /// <exception cref="BankNotFoundException">Thrown when the bank is not found.</exception>
    public async Task<BankResponse> Handle(BankGetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var bank = await repository.FirstOrDefaultAsync(new BankByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);
        _ = bank ?? throw new BankNotFoundException(request.Id);

        return new BankResponse(
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
            bank.ImageUrl);
    }
}

