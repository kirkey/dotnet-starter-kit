using FSH.Starter.WebApi.Store.Application.Categories.Specs;
using FSH.Starter.WebApi.Store.Application.GroceryItems.Specs;

namespace FSH.Starter.WebApi.Store.Application.Categories.Delete.v1;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator(
        [FromKeyedServices("store:categories")] IReadRepository<Category> categories,
        [FromKeyedServices("store:grocery-items")] IReadRepository<GroceryItem> groceryItems)
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
                var hasItems = await groceryItems.AnyAsync(new GroceryItemsByCategoryIdSpec(id), ct);
                return !hasItems;
            })
            .WithMessage("Cannot delete a category that has grocery items.");
    }
}

