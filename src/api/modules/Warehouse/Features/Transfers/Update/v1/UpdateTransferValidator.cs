using FluentValidation;

namespace FSH.Starter.WebApi.Warehouse.Features.Transfers.Update.v1;

public sealed class UpdateTransferValidator : AbstractValidator<UpdateTransferCommand>
{
    public UpdateTransferValidator()
    {
        RuleFor(p => p.Id).NotEmpty();
    }
}

