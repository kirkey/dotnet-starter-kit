using Accounting.Application.TaxCodes.Responses;

namespace Accounting.Application.TaxCodes.Get.v1;

public class GetTaxCodeRequest(DefaultIdType id) : IRequest<TaxCodeResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
