namespace FSH.Starter.WebApi.Store.Application.Items.Get.v1;

public class GetItemCommandValidator : AbstractValidator<GetItemCommand>
{
    public GetItemCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
