namespace FSH.Starter.WebApi.Store.Application.Categories.Create.v1;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator(
        [FromKeyedServices("store:categories")] IReadRepository<Category> categories)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .Must(n => n is null || n.Trim().Length == n.Length)
            .WithMessage("Name cannot start or end with whitespace.")
            .MustAsync(async (name, ct) =>
            {
                if (string.IsNullOrWhiteSpace(name)) return true;
                return await categories.FirstOrDefaultAsync(new Specs.CategoryByNameSpec(name), ct) is null;
            })
            .WithMessage("A category with the same name already exists.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50)
            .Matches(@"^[A-Z0-9]+$")
            .WithMessage("Category code must contain only uppercase letters and numbers")
            .MustAsync(async (code, ct) =>
            {
                if (string.IsNullOrWhiteSpace(code)) return true;
                return await categories.FirstOrDefaultAsync(new Specs.CategoryByCodeSpec(code), ct) is null;
            })
            .WithMessage("A category with the same code already exists.");

        RuleFor(x => x.ParentCategoryId)
            .MustAsync(async (id, ct) =>
            {
                if (!id.HasValue) return true;
                return await categories.GetByIdAsync(id.Value, ct) is not null;
            })
            .WithMessage("Parent category does not exist.");

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.ImageUrl));
    }
}
