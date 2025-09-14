namespace FSH.Starter.WebApi.Store.Application.Categories.Create.v1;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50)
            .Matches(@"^[A-Z0-9]+$")
            .WithMessage("Category code must contain only uppercase letters and numbers");

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0);
    }
}
