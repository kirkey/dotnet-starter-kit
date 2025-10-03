using FSH.Starter.WebApi.Store.Application.Categories.Specs;

namespace FSH.Starter.WebApi.Store.Application.Categories.Delete.v1;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator(
        [FromKeyedServices("store:categories")] IReadRepository<Category> categories,
        [FromKeyedServices("store:items")] IReadRepository<Item> items)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(async (id, ct) => await categories.GetByIdAsync(id, ct) is not null)
            .WithMessage("Category not found.")
            .MustAsync(async (id, ct) =>
            {
                var hasChildren = await categories.AnyAsync(new CategoryChildrenExistSpec(id), ct);
                return !hasChildren;
            })
            .WithMessage("Cannot delete a category that has subcategories.")
            .MustAsync(async (id, ct) =>
            {
                var hasItems = await items.AnyAsync(new ItemsByCategoryIdSpec(id), ct);
                return !hasItems;
            })
            .WithMessage("Cannot delete a category that has grocery items.");
    }
}

