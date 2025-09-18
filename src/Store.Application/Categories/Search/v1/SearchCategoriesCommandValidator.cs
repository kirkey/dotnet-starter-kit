namespace FSH.Starter.WebApi.Store.Application.Categories.Search.v1;

public class SearchCategoriesCommandValidator : AbstractValidator<SearchCategoriesCommand>
{
    public SearchCategoriesCommandValidator()
    {
        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);

        RuleFor(x => x.Name)
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Code)
            .MaximumLength(50)
            .Matches(@"^[A-Z0-9]+$")
            .WithMessage("Code filter must be uppercase alphanumeric.")
            .When(x => !string.IsNullOrEmpty(x.Code));
    }
}

