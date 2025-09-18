namespace Accounting.Application.Accruals.Delete;

public class DeleteAccrualRequestValidator : AbstractValidator<DeleteAccrualRequest>
{
    public DeleteAccrualRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
