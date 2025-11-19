namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Delete.v1;

public sealed class DeletePayComponentHandler(
    ILogger<DeletePayComponentHandler> logger,
    [FromKeyedServices("hr:paycomponents")] IRepository<PayComponent> repository)
    : IRequestHandler<DeletePayComponentCommand, DeletePayComponentResponse>
{
    public async Task<DeletePayComponentResponse> Handle(DeletePayComponentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var payComponent = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = payComponent ?? throw new PayComponentNotFoundException(request.Id);

        await repository.DeleteAsync(payComponent, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Pay component with id {PayComponentId} deleted.", payComponent.Id);

        return new DeletePayComponentResponse(payComponent.Id);
    }
}

