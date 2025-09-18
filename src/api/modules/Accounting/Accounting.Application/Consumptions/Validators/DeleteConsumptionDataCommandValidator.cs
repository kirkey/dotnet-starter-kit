using Accounting.Application.Consumptions.Commands;

namespace Accounting.Application.Consumptions.Validators;

public class DeleteConsumptionDataCommandValidator : AbstractValidator<DeleteConsumptionDataCommand>
{
    public DeleteConsumptionDataCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

