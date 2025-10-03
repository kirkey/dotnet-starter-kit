namespace FSH.Starter.WebApi.Store.Application.PickLists.Get.v1;

public class GetPickListValidator : AbstractValidator<GetPickListCommand>
{
    public GetPickListValidator()
    {
        RuleFor(x => x.PickListId)
            .NotEmpty().WithMessage("Pick list ID is required");
    }
}
