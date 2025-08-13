using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FSH.Starter.Blazor.FluentClient.Components;

public partial class FshValidation : ComponentBase
{
    [Inject] private IToastService ToastService { get; set; } = default!;
    
    private readonly List<string> _errors = new();

    public void DisplayErrors(Dictionary<string, string[]> errors)
    {
        _errors.Clear();
        
        foreach (var error in errors)
        {
            foreach (var errorMessage in error.Value)
            {
                _errors.Add(errorMessage);
            }
        }

        foreach (var error in _errors)
        {
            ToastService.ShowError(error);
        }
        
        StateHasChanged();
    }

    public void ClearErrors()
    {
        _errors.Clear();
        StateHasChanged();
    }

    protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
    {
        if (_errors.Any())
        {
            builder.OpenComponent<FluentMessageBar>(0);
            builder.AddAttribute(1, "Intent", MessageIntent.Error);
            builder.AddAttribute(2, "ChildContent", (RenderFragment)(childBuilder =>
            {
                foreach (var error in _errors)
                {
                    childBuilder.OpenElement(3, "div");
                    childBuilder.AddContent(4, error);
                    childBuilder.CloseElement();
                }
            }));
            builder.CloseComponent();
        }
    }
}
