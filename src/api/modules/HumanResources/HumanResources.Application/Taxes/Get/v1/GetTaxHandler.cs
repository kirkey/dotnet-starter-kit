using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Get.v1;

/// <summary>
/// Handler for GetTaxRequest.
/// Retrieves a tax master configuration by ID.
/// </summary>
public sealed class GetTaxHandler(
    ILogger<GetTaxHandler> logger,
    [FromKeyedServices("hr:taxes")] IReadRepository<TaxMaster> repository)
    : IRequestHandler<GetTaxRequest, TaxResponse>
{
    /// <summary>
    /// Handles the get tax query.
    /// </summary>
    /// <param name="request">Get tax query with tax ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Tax response with complete configuration details.</returns>
    /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
    /// <exception cref="NotFoundException">Thrown when tax is not found.</exception>
    public async Task<TaxResponse> Handle(GetTaxRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tax = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Tax with ID {request.Id} not found");

        logger.LogInformation(
            "Retrieved tax with ID {TaxId}, Code {TaxCode}",
            tax.Id,
            tax.Code);

        return new TaxResponse(
            Id: tax.Id,
            Code: tax.Code,
            Name: tax.Name,
            TaxType: tax.TaxType,
            Rate: tax.Rate,
            IsCompound: tax.IsCompound,
            Jurisdiction: tax.Jurisdiction,
            EffectiveDate: tax.EffectiveDate,
            ExpiryDate: tax.ExpiryDate,
            TaxCollectedAccountId: tax.TaxCollectedAccountId,
            TaxPaidAccountId: tax.TaxPaidAccountId,
            TaxAuthority: tax.TaxAuthority,
            TaxRegistrationNumber: tax.TaxRegistrationNumber,
            ReportingCategory: tax.ReportingCategory,
            IsActive: tax.IsActive,
            CreatedOn: tax.CreatedOn,
            CreatedBy: tax.CreatedBy,
            LastModifiedOn: tax.LastModifiedOn,
            LastModifiedBy: tax.LastModifiedBy);
    }
}

