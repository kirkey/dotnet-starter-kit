using FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Get.v1;

/// <summary>
/// Handler for retrieving a payroll line by ID.
/// </summary>
public sealed class GetPayrollLineHandler(
    [FromKeyedServices("hr:payrolllines")] IReadRepository<PayrollLine> repository)
    : IRequestHandler<GetPayrollLineRequest, PayrollLineResponse>
{
    public async Task<PayrollLineResponse> Handle(
        GetPayrollLineRequest request,
        CancellationToken cancellationToken)
    {
        var line = await repository
            .FirstOrDefaultAsync(new PayrollLineByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (line is null)
            throw new Exception($"Payroll line not found: {request.Id}");

        return MapToResponse(line);
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

