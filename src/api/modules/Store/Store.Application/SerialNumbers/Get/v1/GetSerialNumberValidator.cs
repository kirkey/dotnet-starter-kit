namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Get.v1;

public class GetSerialNumberValidator : AbstractValidator<GetSerialNumberCommand>
{
    public GetSerialNumberValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Serial number ID is required.");
    }
}
