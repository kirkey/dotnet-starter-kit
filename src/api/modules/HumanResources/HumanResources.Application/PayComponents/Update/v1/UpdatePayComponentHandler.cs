namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Update.v1;

/// <summary>
/// Handler for updating pay component.
/// </summary>
public sealed class UpdatePayComponentHandler(
    ILogger<UpdatePayComponentHandler> logger,
    [FromKeyedServices("hr:paycomponents")] IRepository<PayComponent> repository)
    : IRequestHandler<UpdatePayComponentCommand, UpdatePayComponentResponse>
{
    public async Task<UpdatePayComponentResponse> Handle(
        UpdatePayComponentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var component = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (component is null)
            throw new PayComponentNotFoundException(request.Id);

        // Update fields if provided
        if (!string.IsNullOrWhiteSpace(request.ComponentName) || 
            !string.IsNullOrWhiteSpace(request.GlAccountCode) || 
            !string.IsNullOrWhiteSpace(request.Description))
        {
            component.Update(
                request.ComponentName,
                request.GlAccountCode,
                request.Description);
        }

        // Update active status if provided
        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
                component.Activate();
            else
                component.Deactivate();
        }

        await repository.UpdateAsync(component, cancellationToken);

        logger.LogInformation(
            "Pay component {Id} updated: {Name}, Active: {Active}",
            component.Id,
            component.ComponentName,
            component.IsActive);

        return new UpdatePayComponentResponse(
            component.Id,
            component.ComponentName,
            component.IsActive);
    }
}

