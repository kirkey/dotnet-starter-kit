using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Get.v1;

public sealed class CreditBureauInquiryByIdSpec : Specification<CreditBureauInquiry>, ISingleResultSpecification<CreditBureauInquiry>
{
    public CreditBureauInquiryByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}
