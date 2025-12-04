using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Specifications;

public sealed class FixedDepositByCertificateNumberSpec : Specification<FixedDeposit>, ISingleResultSpecification<FixedDeposit>
{
    public FixedDepositByCertificateNumberSpec(string certificateNumber)
    {
        Query.Where(fd => fd.CertificateNumber == certificateNumber);
    }
}
