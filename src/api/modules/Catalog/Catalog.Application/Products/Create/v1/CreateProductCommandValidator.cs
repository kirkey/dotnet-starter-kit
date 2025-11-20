﻿using FluentValidation;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Catalog.Application.Products.Specifications;
using FSH.Starter.WebApi.Catalog.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Catalog.Application.Products.Create.v1;

/// <summary>
/// Validator for CreateProductCommand with comprehensive validation rules.
/// Validates name length, uniqueness, price range, brand existence, and enforces business rules.
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator(
        [FromKeyedServices("catalog:products")] IReadRepository<Product> productRepository,
        [FromKeyedServices("catalog:brands")] IReadRepository<Brand> brandRepository)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("Product name is required.")
            .MinimumLength(Product.NameMinLength)
            .MaximumLength(Product.NameMaxLength)
            .MustAsync(async (name, ct) =>
            {
                if (string.IsNullOrWhiteSpace(name)) return true;
                var existingProduct = await productRepository.FirstOrDefaultAsync(
                    new ProductByNameSpec(name.Trim()), ct).ConfigureAwait(false);
                return existingProduct is null;
            })
            .WithMessage("Product with this name already exists.");

        RuleFor(p => p.Price)
            .GreaterThanOrEqualTo(Product.MinPrice)
            .LessThanOrEqualTo(Product.MaxPrice);

        RuleFor(p => p.Description)
            .MaximumLength(Product.DescriptionMaxLength)
            .When(p => !string.IsNullOrWhiteSpace(p.Description));

        RuleFor(p => p.BrandId)
            .MustAsync(async (brandId, ct) =>
            {
                if (!brandId.HasValue || brandId.Value == DefaultIdType.Empty) return true;
                var brand = await brandRepository.GetByIdAsync(brandId.Value, ct).ConfigureAwait(false);
                return brand is not null;
            })
            .WithMessage("Selected brand does not exist.")
            .When(p => p.BrandId.HasValue && p.BrandId.Value != DefaultIdType.Empty);
    }
}
