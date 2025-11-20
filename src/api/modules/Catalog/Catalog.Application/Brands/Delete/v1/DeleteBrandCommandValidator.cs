using FluentValidation;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Catalog.Application.Products.Specifications;
using FSH.Starter.WebApi.Catalog.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Catalog.Application.Brands.Delete.v1;

/// <summary>
/// Validator for DeleteBrandCommand.
/// Ensures the brand exists and has no associated products before deletion.
/// </summary>
public class DeleteBrandCommandValidator : AbstractValidator<DeleteBrandCommand>
{
    public DeleteBrandCommandValidator(
        [FromKeyedServices("catalog:brands")] IReadRepository<Brand> brandRepository,
        [FromKeyedServices("catalog:products")] IReadRepository<Product> productRepository)
    {
        RuleFor(b => b.Id)
            .NotEmpty()
            .WithMessage("Brand ID is required.")
            .MustAsync(async (id, ct) =>
            {
                var brand = await brandRepository.GetByIdAsync(id, ct).ConfigureAwait(false);
                return brand is not null;
            })
            .WithMessage("Brand not found.")
            .MustAsync(async (id, ct) =>
            {
                var hasProducts = await productRepository.AnyAsync(
                    new ProductsByBrandSpec(id), ct).ConfigureAwait(false);
                return !hasProducts;
            })
            .WithMessage("Cannot delete brand because it has associated products. Please delete or reassign the products first.");
    }
}

