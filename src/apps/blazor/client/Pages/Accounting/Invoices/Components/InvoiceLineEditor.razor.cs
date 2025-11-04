namespace FSH.Starter.Blazor.Client.Pages.Accounting.Invoices.Components;

/// <summary>
/// Code-behind for Invoice Line Item Editor component.
/// </summary>
public partial class InvoiceLineEditor
{
    /// <summary>
    /// Collection of invoice line items to edit.
    /// </summary>
    [Parameter]
    public List<InvoiceLineItemViewModel> Lines { get; set; } = new();

    /// <summary>
    /// Event callback when lines are modified.
    /// </summary>
    [Parameter]
    public EventCallback<List<InvoiceLineItemViewModel>> LinesChanged { get; set; }

    /// <summary>
    /// Whether the editor is in read-only mode.
    /// </summary>
    [Parameter]
    public bool IsReadOnly { get; set; }

    /// <summary>
    /// Total amount of all line items.
    /// </summary>
    private decimal TotalAmount => Lines.Sum(l => l.TotalPrice);

    /// <summary>
    /// Adds a new line item to the collection.
    /// </summary>
    private async Task AddLine()
    {
        var newLine = new InvoiceLineItemViewModel
        {
            Description = string.Empty,
            Quantity = 1,
            UnitPrice = 0,
            TotalPrice = 0
        };
        Lines.Add(newLine);
        await LinesChanged.InvokeAsync(Lines);
    }

    /// <summary>
    /// Removes a line item from the collection.
    /// </summary>
    private async Task RemoveLine(InvoiceLineItemViewModel line)
    {
        Lines.Remove(line);
        await LinesChanged.InvokeAsync(Lines);
    }
}

