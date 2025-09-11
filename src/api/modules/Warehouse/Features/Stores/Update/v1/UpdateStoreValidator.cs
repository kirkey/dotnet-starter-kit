using FluentValidation;

namespace FSH.Starter.WebApi.Warehouse.Features.Stores.Update.v1;

public sealed class UpdateStoreValidator : AbstractValidator<UpdateStoreCommand>
{
    public UpdateStoreValidator()
    {
        RuleFor(p => p.Id).NotEmpty();
    }
}

