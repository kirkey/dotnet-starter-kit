namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Create.v1;

public sealed class CreatePayComponentHandler(
    ILogger<CreatePayComponentHandler> logger,
    [FromKeyedServices("hr:paycomponents")] IRepository<PayComponent> repository)
    : IRequestHandler<CreatePayComponentCommand, CreatePayComponentResponse>
{
    public async Task<CreatePayComponentResponse> Handle(CreatePayComponentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var payComponent = PayComponent.Create(
            request.Code,
            request.ComponentName,
            request.ComponentType,
            request.CalculationMethod,
            request.GlAccountCode ?? string.Empty);

        // Set optional properties
        payComponent.Update(
            calculationFormula: request.CalculationFormula,
            rate: request.Rate,
            fixedAmount: request.FixedAmount,
            description: request.Description,
            displayOrder: request.DisplayOrder);

        if (request.MinValue.HasValue || request.MaxValue.HasValue)
        {
            payComponent.SetLimits(request.MinValue, request.MaxValue);
        }

        if (request.IsMandatory)
        {
            payComponent.SetMandatory(request.LaborLawReference);
        }

        payComponent.SetTaxTreatment(request.IsSubjectToTax, request.IsTaxExempt);
        payComponent.SetPayImpact(request.AffectsGrossPay, request.AffectsNetPay);
        payComponent.SetAutoCalculated(request.IsCalculated);

        await repository.AddAsync(payComponent, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Pay component created {PayComponentId} with code {Code}", payComponent.Id, payComponent.Code);

        return new CreatePayComponentResponse(payComponent.Id);
    }
}

