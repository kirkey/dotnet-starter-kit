using FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Search.v1;

/// <summary>
/// Handler for searching payroll lines.
/// </summary>
public sealed class SearchPayrollLinesHandler(
    [FromKeyedServices("hr:payrolllines")] IReadRepository<PayrollLine> repository)
    : IRequestHandler<SearchPayrollLinesRequest, PagedList<PayrollLineResponse>>
{
    public async Task<PagedList<PayrollLineResponse>> Handle(
        SearchPayrollLinesRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new SearchPayrollLinesSpec(request);
        var lines = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = lines.Select(MapToResponse).ToList();

        return new PagedList<PayrollLineResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }

    private static PayrollLineResponse MapToResponse(PayrollLine line)
    {
        return new PayrollLineResponse
        {
            Id = line.Id,
            PayrollId = line.PayrollId,
            EmployeeId = line.EmployeeId,
            RegularHours = line.RegularHours,
            OvertimeHours = line.OvertimeHours,
            RegularPay = line.RegularPay,
            OvertimePay = line.OvertimePay,
            BonusPay = line.BonusPay,
            OtherEarnings = line.OtherEarnings,
            GrossPay = line.GrossPay,
            IncomeTax = line.IncomeTax,
            SocialSecurityTax = line.SocialSecurityTax,
            MedicareTax = line.MedicareTax,
            OtherTaxes = line.OtherTaxes,
            TotalTaxes = line.TotalTaxes,
            HealthInsurance = line.HealthInsurance,
            RetirementContribution = line.RetirementContribution,
            OtherDeductions = line.OtherDeductions,
            TotalDeductions = line.TotalDeductions,
            NetPay = line.NetPay,
            PaymentMethod = line.PaymentMethod,
            BankAccountLast4 = line.BankAccountLast4,
            CheckNumber = line.CheckNumber
        };
    }
}

