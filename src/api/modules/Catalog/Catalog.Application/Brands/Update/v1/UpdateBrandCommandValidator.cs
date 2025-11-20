using FluentValidation;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Catalog.Application.Brands.Specifications;
using FSH.Starter.WebApi.Catalog.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Catalog.Application.Brands.Update.v1;

/// <summary>
/// Validator for UpdateBrandCommand with comprehensive validation rules.
/// Validates name length, uniqueness (excluding current record), description length, and enforces business rules.
/// </summary>
public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
{
    public UpdateBrandCommandValidator(
        [FromKeyedServices("catalog:brands")] IReadRepository<Brand> repository)
    {
        RuleFor(b => b.Id)
            .NotEmpty()
            .WithMessage("Brand ID is required.");

        RuleFor(b => b.Name)
            .NotEmpty()
            .WithMessage("Brand name is required.")
            .MinimumLength(Brand.NameMinLength)
            .MaximumLength(Brand.NameMaxLength)
            .MustAsync(async (command, name, ct) =>
            {
                if (string.IsNullOrWhiteSpace(name)) return true;
                var existingBrand = await repository.FirstOrDefaultAsync(
                    new BrandByNameSpec(name.Trim()), ct).ConfigureAwait(false);
                return existingBrand is null || existingBrand.Id == command.Id;
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

