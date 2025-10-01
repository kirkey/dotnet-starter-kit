namespace FSH.Starter.WebApi.Store.Application.Categories.Get.v1;

public class GetCategoryRequestValidator : AbstractValidator<GetCategoryRequest>
{
    public GetCategoryRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

