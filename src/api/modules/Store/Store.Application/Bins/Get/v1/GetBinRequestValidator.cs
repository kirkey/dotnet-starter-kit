namespace FSH.Starter.WebApi.Store.Application.Bins.Get.v1;

public class GetBinRequestValidator : AbstractValidator<GetBinRequest>
{
    public GetBinRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
