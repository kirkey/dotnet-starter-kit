namespace FSH.Starter.Blazor.Client.Pages.Accounting.Bills.Components;

/// <summary>
/// Component for editing bill line items with inline grid editing.
/// Supports adding/removing line items and real-time total calculation.
/// </summary>
public partial class BillLineEditor
{
    /// <summary>
    /// The list of bill line items to edit.
    /// </summary>
    [Parameter]
    public List<BillLineItemViewModel> Lines { get; set; } = new();

    /// <summary>
    /// Event callback when lines are modified.
    /// </summary>
    [Parameter]
    public EventCallback<List<BillLineItemViewModel>> LinesChanged { get; set; }

    /// <summary>
    /// Indicates whether the editor is in read-only mode.
    /// </summary>
    [Parameter]
    public bool IsReadOnly { get; set; }

    /// <summary>
    /// Subtotal amount (sum of all line amounts excluding tax).
    /// </summary>
    private decimal SubtotalAmount => Lines.Sum(l => l.Amount);

    /// <summary>
    /// Total tax amount across all lines.
    /// </summary>
    private decimal TotalTax => Lines.Sum(l => l.TaxAmount);

    /// <summary>
    /// Total amount including tax.
    /// </summary>
    private decimal TotalAmount => SubtotalAmount + TotalTax;

    /// <summary>
    /// Adds a new line item to the bill.
    /// </summary>
    public async Task AddLine()
    {
        var newLineNumber = Lines.Count > 0 ? Lines.Max(l => l.LineNumber) + 1 : 1;
        
        Lines.Add(new BillLineItemViewModel
        {
            LineNumber = newLineNumber,
            Quantity = 1,
            UnitPrice = 0,
            Amount = 0
        });
        
        await LinesChanged.InvokeAsync(Lines);
    }

    /// <summary>
    /// Removes a line item from the bill.
    /// </summary>
    public async Task RemoveLine(BillLineItemViewModel line)
    {
        Lines.Remove(line);
        await LinesChanged.InvokeAsync(Lines);
    }
}

