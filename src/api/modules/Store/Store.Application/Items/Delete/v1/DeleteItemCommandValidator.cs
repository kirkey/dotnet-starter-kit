namespace FSH.Starter.WebApi.Store.Application.Items.Delete.v1;

public class DeleteItemCommandValidator : AbstractValidator<DeleteItemCommand>
{
    public DeleteItemCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
