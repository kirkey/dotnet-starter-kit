using FSH.Starter.Blazor.Infrastructure.Api;

namespace FSH.Starter.Blazor.Client.Pages.Store.Items;

public partial class AddItemToSupplierDialog
{
    [Parameter]
    public DefaultIdType ItemId { get; set; }

    [Parameter]
    public string ItemName { get; set; } = string.Empty;

    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private MudForm _form = null!;
    private string _itemName = string.Empty;

    private CreateItemSupplierCommand Model { get; set; } = new()
    {
        UnitCost = 0,
        LeadTimeDays = 7,
        MinimumOrderQuantity = 1,
        IsPreferred = false
    };

    protected override void OnInitialized()
    {
        _itemName = ItemName;
        Model.ItemId = ItemId;
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async Task Submit()
    {
        await _form.Validate();

        if (!_form.IsValid)
        {
            return;
        }

        try
        {
            await Client.CreateItemSupplierEndpointAsync("1", Model);
            Snackbar.Add($"Item successfully added to supplier", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (ApiException ex) when (ex.StatusCode == 400)
        {
            // Handle duplicate ItemSupplier error
            Snackbar.Add($"This item is already linked to the selected supplier", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error adding item to supplier: {ex.Message}", Severity.Error);
        }
    }
}
