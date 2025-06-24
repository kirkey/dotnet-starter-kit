using Accounting.Application.Payees.Queries;
using Accounting.Domain;
using FluentValidation;
using FSH.Framework.Core.Persistence;

namespace Accounting.Application.Payees.Create.v1;

public class PayeeCreateCommandValidator : AbstractValidator<PayeeCreateCommand>
{
    public PayeeCreateCommandValidator(IReadRepository<Payee> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        RuleFor(p => p.PayeeCode)
            .NotEmpty()
            .MaximumLength(32)
            .MustAsync(async (code, cancellation) =>
                (await repository.FirstOrDefaultAsync(new PayeeByCodeSpec(code), cancellation)) == null)
            .WithMessage("Payee Code already exists.");

        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(1024)
            .MustAsync(async (name, cancellation) =>
                (await repository.FirstOrDefaultAsync(new PayeeByNameSpec(name), cancellation)) == null)
            .WithMessage("Payee Name already exists.");
    }
}
