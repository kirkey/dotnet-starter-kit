namespace FSH.Starter.WebApi.Store.Application.Categories.Update.v1;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator(
        [FromKeyedServices("store:categories")] IReadRepository<Category> categories)
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name)
            .MaximumLength(200)
            .Must(n => n is null || n.Trim().Length == n.Length)
            .WithMessage("Name cannot start or end with whitespace.")
            .MustAsync(async (request, name, ct) =>
            {
                if (string.IsNullOrWhiteSpace(name)) return true;
                var existing = await categories.FirstOrDefaultAsync(new FSH.Starter.WebApi.Store.Application.Categories.Specs.CategoryByNameSpec(name, request.Id), ct);
                return existing is null;
            })
            .WithMessage("A category with the same name already exists.")
            .When(x => x.Name is not null);

        RuleFor(x => x.Code)
            .MaximumLength(50)
            .Matches(@"^[A-Z0-9]+$")
            .WithMessage("Category code must contain only uppercase letters and numbers")
            .MustAsync(async (request, code, ct) =>
            {
                if (string.IsNullOrWhiteSpace(code)) return true;
                var existing = await categories.FirstOrDefaultAsync(new FSH.Starter.WebApi.Store.Application.Categories.Specs.CategoryByCodeSpec(code, request.Id), ct);
                return existing is null;
            })
            .WithMessage("A category with the same code already exists.")
            .When(x => x.Code is not null);

        RuleFor(x => x.ParentCategoryId)
            .MustAsync(async (request, parentId, ct) =>
            {
                if (!parentId.HasValue) return true;
                if (parentId.Value == request.Id) return false;
                return await categories.GetByIdAsync(parentId.Value, ct) is not null;
            })
            .WithMessage("Parent category must exist and cannot be the category itself.");

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0)
            .When(x => x.SortOrder.HasValue);

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.ImageUrl));
    }
}
