namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Delete.v1;

public class DeleteSerialNumberValidator : AbstractValidator<DeleteSerialNumberCommand>
{
    public DeleteSerialNumberValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Serial number ID is required.");
    }
}
