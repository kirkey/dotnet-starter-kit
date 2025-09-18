using Accounting.Application.Consumptions.Commands;

namespace Accounting.Application.Consumptions.Validators;

public class DeleteConsumptionCommandValidator : AbstractValidator<DeleteConsumptionCommand>
{
    public DeleteConsumptionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

