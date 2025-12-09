using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Update.v1;

/// <summary>
/// Handler for updating a collection action.
/// </summary>
public sealed class UpdateCollectionActionHandler(
    ILogger<UpdateCollectionActionHandler> logger,
    [FromKeyedServices("microfinance:collectionactions")] IRepository<CollectionAction> repository)
    : IRequestHandler<UpdateCollectionActionCommand, UpdateCollectionActionResponse>
{
    public async Task<UpdateCollectionActionResponse> Handle(UpdateCollectionActionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var collectionAction = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (collectionAction is null)
        {
            throw new NotFoundException($"Collection action with ID {request.Id} not found.");
        }

        collectionAction.Update(
            outcome: request.Outcome,
            description: request.Description,
            contactMethod: request.ContactMethod,
            phoneNumberCalled: request.PhoneNumberCalled,
            contactPerson: request.ContactPerson,
            promisedAmount: request.PromisedAmount,
            promisedDate: request.PromisedDate,
            nextFollowUpDate: request.NextFollowUpDate,
            durationMinutes: request.DurationMinutes,
            latitude: request.Latitude,
            longitude: request.Longitude,
            notes: request.Notes);

        await repository.UpdateAsync(collectionAction, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Collection action updated with ID: {Id}", collectionAction.Id);

        return new UpdateCollectionActionResponse(collectionAction.Id);
    }
}
