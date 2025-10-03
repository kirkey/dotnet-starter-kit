namespace FSH.Starter.WebApi.Store.Application.Categories.Get.v1;

public class GetCategoryCommandValidator : AbstractValidator<GetCategoryCommand>
{
    public GetCategoryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

