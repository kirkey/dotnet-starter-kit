using Accounting.Application.TaxCodes.Responses;

namespace Accounting.Application.TaxCodes.Get.v1;

public sealed class GetTaxCodeHandler(
    IReadRepository<TaxCode> repository)
    : IRequestHandler<GetTaxCodeRequest, TaxCodeResponse>
{
    public async Task<TaxCodeResponse> Handle(GetTaxCodeRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var taxCode = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (taxCode == null)
            throw new TaxCodeNotFoundException(request.Id);

        return new TaxCodeResponse
        {
            Id = taxCode.Id,
            Code = taxCode.Code,
            Name = taxCode.Name,
            TaxType = taxCode.TaxType.ToString(),
            Rate = taxCode.Rate,
            IsCompound = taxCode.IsCompound,
            Jurisdiction = taxCode.Jurisdiction,
            EffectiveDate = taxCode.EffectiveDate,
            ExpiryDate = taxCode.ExpiryDate,
            IsActive = taxCode.IsActive,
            TaxCollectedAccountId = taxCode.TaxCollectedAccountId,
            TaxPaidAccountId = taxCode.TaxPaidAccountId,
            TaxAuthority = taxCode.TaxAuthority,
            TaxRegistrationNumber = taxCode.TaxRegistrationNumber,
            ReportingCategory = taxCode.ReportingCategory,
            Description = taxCode.Description,
            CreatedOn = taxCode.CreatedOn,
            CreatedBy = taxCode.CreatedBy,
            LastModifiedOn = taxCode.LastModifiedOn,
            LastModifiedBy = taxCode.LastModifiedBy
        };
    }
}
