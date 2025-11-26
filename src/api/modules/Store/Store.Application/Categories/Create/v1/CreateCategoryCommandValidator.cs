using FSH.Starter.WebApi.Store.Application.Categories.Specs;

namespace FSH.Starter.WebApi.Store.Application.Categories.Create.v1;

/// <summary>
/// Validator for CreateCategoryCommand with comprehensive validation rules.
/// Validates field lengths using domain constants, uniqueness checks for Name and Code, and enforces business rules.
/// </summary>
public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator(
        [FromKeyedServices("store:categories")] IReadRepository<Category> categories)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(Category.NameMinLength)
            .MaximumLength(Category.NameMaxLength)
            .Must(n => n is null || n.Trim().Length == n.Length)
            .WithMessage("Name cannot start or end with whitespace.")
            .MustAsync(async (name, ct) =>
            {
                if (string.IsNullOrWhiteSpace(name)) return true;
                return await categories.FirstOrDefaultAsync(new CategoryByNameSpec(name), ct).ConfigureAwait(false) is null;
            })
            .WithMessage("A category with the same name already exists.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(Category.CodeMaxLength)
            .Matches(@"^[A-Z0-9]+$")
            .WithMessage("Category code must contain only uppercase letters and numbers")
            .MustAsync(async (code, ct) =>
            {
                if (string.IsNullOrWhiteSpace(code)) return true;
                return await categories.FirstOrDefaultAsync(new CategoryByCodeSpec(code), ct).ConfigureAwait(false) is null;
            })
            .WithMessage("A category with the same code already exists.");

        RuleFor(x => x.ParentCategoryId)
            .MustAsync(async (id, ct) =>
            {
                if (!id.HasValue) return true;
                return await categories.GetByIdAsync(id.Value, ct).ConfigureAwait(false) is not null;
            })
            .WithMessage("Parent category does not exist.");

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.ImageUrl)
            .MaximumLength(Category.ImageUrlMaxLength)
            .When(x => !string.IsNullOrEmpty(x.ImageUrl));

        RuleFor(x => x.Description)
            .MaximumLength(Category.DescriptionMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(Category.NotesMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}
