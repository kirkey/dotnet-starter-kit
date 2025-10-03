namespace FSH.Starter.WebApi.Store.Application.Bins.Delete.v1;

public class DeleteBinCommandValidator : AbstractValidator<DeleteBinCommand>
{
    public DeleteBinCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
