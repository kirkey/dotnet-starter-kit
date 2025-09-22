#if false
namespace Accounting.Application.Accruals.Delete;

public class DeleteAccrualRequestValidator : AbstractValidator<DeleteAccrualCommand>
{
    public DeleteAccrualRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
#endif
