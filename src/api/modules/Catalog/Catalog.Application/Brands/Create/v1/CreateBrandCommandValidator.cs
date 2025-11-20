﻿using FluentValidation;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Catalog.Application.Brands.Specifications;
using FSH.Starter.WebApi.Catalog.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Catalog.Application.Brands.Create.v1;

/// <summary>
/// Validator for CreateBrandCommand with comprehensive validation rules.
/// Validates name length, uniqueness, description length, and enforces business rules.
/// </summary>
public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    public CreateBrandCommandValidator(
        [FromKeyedServices("catalog:brands")] IReadRepository<Brand> repository)
    {
        RuleFor(b => b.Name)
            .NotEmpty()
            .WithMessage("Brand name is required.")
            .MinimumLength(Brand.NameMinLength)
            .MaximumLength(Brand.NameMaxLength)
            .MustAsync(async (name, ct) =>
            {
                if (string.IsNullOrWhiteSpace(name)) return true;
                var existingBrand = await repository.FirstOrDefaultAsync(
                    new BrandByNameSpec(name.Trim()), ct).ConfigureAwait(false);
                return existingBrand is null;
            })
            .WithMessage("Brand with this name already exists.");

        RuleFor(b => b.Description)
            .MaximumLength(Brand.DescriptionMaxLength)
            .When(b => !string.IsNullOrWhiteSpace(b.Description));

        RuleFor(b => b.Notes)
            .MaximumLength(Brand.NotesMaxLength)
            .When(b => !string.IsNullOrWhiteSpace(b.Notes));
    }
}
