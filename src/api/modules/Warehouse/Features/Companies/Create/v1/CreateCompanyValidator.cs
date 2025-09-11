using FluentValidation;

namespace FSH.Starter.WebApi.Warehouse.Features.Companies.Create.v1;

public sealed class CreateCompanyValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}
