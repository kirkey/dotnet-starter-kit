namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.Get.v1;

public class GetInventoryReservationValidator : AbstractValidator<GetInventoryReservationCommand>
{
    public GetInventoryReservationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Reservation ID is required.");
    }
}
