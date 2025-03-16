using FluentValidation;
using FSH.Framework.Core.Extensions.Dto;

namespace FSH.Framework.Core.Extensions;

public class BaseRequestValidator<T> : AbstractValidator<T> where T : BaseRequest
{
    protected BaseRequestValidator()
    {
        RuleFor(a => a.Type)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(64);
        
        RuleFor(a => a.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(1024);
    
        RuleFor(a => a.Description)
            .MaximumLength(2048);
    }
}
