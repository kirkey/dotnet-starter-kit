namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.Delete.v1;

public class DeleteInventoryReservationValidator : AbstractValidator<DeleteInventoryReservationCommand>
{
    public DeleteInventoryReservationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Reservation ID is required.");
    }
}
