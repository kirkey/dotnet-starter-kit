using FluentValidation;

namespace FSH.Starter.WebApi.Warehouse.Features.Companies.Update.v1;

public sealed class UpdateCompanyValidator : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyValidator()
    {
        RuleFor(p => p.Id).NotEmpty();
    }
}

