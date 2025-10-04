using Store.Domain.Exceptions.ItemSupplier;

namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Update.v1;

/// <summary>
/// Handler for updating an item-supplier relationship.
/// </summary>
public sealed class UpdateItemSupplierHandler(
    [FromKeyedServices("store:itemsuppliers")] IRepository<ItemSupplier> repository,
    [FromKeyedServices("store:itemsuppliers")] IReadRepository<ItemSupplier> readRepository)
    : IRequestHandler<UpdateItemSupplierCommand, UpdateItemSupplierResponse>
{
    public async Task<UpdateItemSupplierResponse> Handle(UpdateItemSupplierCommand request, CancellationToken cancellationToken)
    {
        var itemSupplier = await readRepository.GetByIdAsync(request.Id, cancellationToken);

        if (itemSupplier is null)
        {
            throw new ItemSupplierNotFoundException(request.Id);
        }

        // Update pricing if provided
        if (request.UnitCost.HasValue)
        {
            itemSupplier.UpdatePricing(request.UnitCost.Value);
        }

        // Update lead time if provided
        if (request.LeadTimeDays.HasValue)
        {
            itemSupplier.UpdateLeadTime(request.LeadTimeDays.Value);
        }

        // Update preferred status if provided
        if (request.IsPreferred.HasValue)
        {
            itemSupplier.SetPreferred(request.IsPreferred.Value);
        }

        // Update reliability rating if provided
        if (request.ReliabilityRating.HasValue)
        {
            itemSupplier.UpdateReliabilityRating(request.ReliabilityRating.Value);
        }

        // Update active status if provided
        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
            {
                itemSupplier.Activate();
            }
            else
            {
                itemSupplier.Deactivate();
            }
        }

        // Update description and notes if provided
        if (!string.IsNullOrWhiteSpace(request.Description) || !string.IsNullOrWhiteSpace(request.Notes))
        {
            itemSupplier.UpdateDetails(request.Description, request.Notes);
        }

        await repository.UpdateAsync(itemSupplier, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new UpdateItemSupplierResponse(itemSupplier.Id);
    }
}
