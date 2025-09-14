using Accounting.Application.ConsumptionData.Commands;

namespace Accounting.Application.ConsumptionData.Validators;

public class DeleteConsumptionDataCommandValidator : AbstractValidator<DeleteConsumptionDataCommand>
{
    public DeleteConsumptionDataCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

