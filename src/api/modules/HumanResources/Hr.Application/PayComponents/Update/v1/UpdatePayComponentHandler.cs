namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Update.v1;

public sealed class UpdatePayComponentHandler(
    ILogger<UpdatePayComponentHandler> logger,
    [FromKeyedServices("hr:paycomponents")] IRepository<PayComponent> repository)
    : IRequestHandler<UpdatePayComponentCommand, UpdatePayComponentResponse>
{
    public async Task<UpdatePayComponentResponse> Handle(UpdatePayComponentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var payComponent = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = payComponent ?? throw new PayComponentNotFoundException(request.Id);

        payComponent.Update(
            request.ComponentName,
            request.CalculationMethod,
            request.CalculationFormula,
            request.Rate,
            request.FixedAmount,
            request.GlAccountCode,
            request.Description,
            request.DisplayOrder);

        await repository.UpdateAsync(payComponent, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Pay component with id : {PayComponentId} updated.", payComponent.Id);

        return new UpdatePayComponentResponse(payComponent.Id);
    }
}

