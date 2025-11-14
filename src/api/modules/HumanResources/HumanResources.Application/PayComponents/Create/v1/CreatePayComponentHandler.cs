namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Create.v1;

/// <summary>
/// Handler for creating pay component with validation.
/// </summary>
public sealed class CreatePayComponentHandler(
    ILogger<CreatePayComponentHandler> logger,
    [FromKeyedServices("hr:paycomponents")] IRepository<PayComponent> repository)
    : IRequestHandler<CreatePayComponentCommand, CreatePayComponentResponse>
{
    public async Task<CreatePayComponentResponse> Handle(
        CreatePayComponentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var component = PayComponent.Create(
            request.ComponentName,
            request.ComponentType,
            request.GlAccountCode);

        if (!string.IsNullOrWhiteSpace(request.Description))
            component.Update(description: request.Description);

        await repository.AddAsync(component, cancellationToken);

        logger.LogInformation(
            "Pay component created: ID {Id}, Name {Name}, Type {Type}",
            component.Id,
            component.ComponentName,
            component.ComponentType);

        return new CreatePayComponentResponse(
            component.Id,
            component.ComponentName,
            component.ComponentType,
            component.IsActive);
    }
}

