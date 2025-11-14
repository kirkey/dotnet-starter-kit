namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Delete.v1;

/// <summary>
/// Handler for deleting pay component.
/// </summary>
public sealed class DeletePayComponentHandler(
    ILogger<DeletePayComponentHandler> logger,
    [FromKeyedServices("hr:paycomponents")] IRepository<PayComponent> repository)
    : IRequestHandler<DeletePayComponentCommand, DeletePayComponentResponse>
{
    public async Task<DeletePayComponentResponse> Handle(
        DeletePayComponentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var component = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (component is null)
            throw new PayComponentNotFoundException(request.Id);

        await repository.DeleteAsync(component, cancellationToken);

        logger.LogInformation("Pay component {Id} deleted", component.Id);

        return new DeletePayComponentResponse(component.Id, true);
    }
}

