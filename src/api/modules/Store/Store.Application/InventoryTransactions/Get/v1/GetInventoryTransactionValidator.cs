namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Get.v1;

public class GetInventoryTransactionValidator : AbstractValidator<GetInventoryTransactionCommand>
{
    public GetInventoryTransactionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Transaction ID is required.");
    }
}
