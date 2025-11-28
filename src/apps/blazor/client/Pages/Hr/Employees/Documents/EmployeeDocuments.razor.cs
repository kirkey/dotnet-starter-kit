namespace FSH.Starter.Blazor.Client.Pages.Hr.Employees.Documents;

public partial class EmployeeDocuments
{
    protected EntityServerTableContext<EmployeeDocumentResponse, DefaultIdType, EmployeeDocumentViewModel> Context { get; set; } = null!;

    [SupplyParameterFromQuery]
    public string? EmployeeId { get; set; }

    public string FilterEmployeeId => EmployeeId ?? string.Empty;

    public string FilterSuffix => !string.IsNullOrEmpty(FilterEmployeeId) ? " (Filtered)" : string.Empty;

    protected override async Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<EmployeeDocumentResponse, DefaultIdType, EmployeeDocumentViewModel>(
            entityName: "Employee Document",
            entityNamePlural: "Employee Documents",
            entityResource: FshResources.Employees,
            fields:
            [
                new EntityField<EmployeeDocumentResponse>(response => response.DocumentType, "Document Type", "DocumentType"),
                new EntityField<EmployeeDocumentResponse>(response => response.Title, "Title", "Title"),
                new EntityField<EmployeeDocumentResponse>(response => response.FileName, "File Name", "FileName"),
                new EntityField<EmployeeDocumentResponse>(response => response.IssueNumber, "Issue Number", "IssueNumber"),
                new EntityField<EmployeeDocumentResponse>(response => response.IssueDate, "Issue Date", "IssueDate", typeof(DateOnly?)),
                new EntityField<EmployeeDocumentResponse>(response => response.ExpiryDate, "Expiry Date", "ExpiryDate", typeof(DateOnly?)),
                new EntityField<EmployeeDocumentResponse>(response => response.UploadedDate, "Uploaded Date", "UploadedDate", typeof(DateOnly)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchEmployeeDocumentsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };

                // Filter by EmployeeId if provided
                if (!string.IsNullOrEmpty(FilterEmployeeId) && Guid.TryParse(FilterEmployeeId, out var employeeGuid))
                {
                    request.EmployeeId = employeeGuid;
                }

                var result = await Client.SearchEmployeeDocumentsEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<EmployeeDocumentResponse>>();
            },
            createFunc: async document =>
            {
                var command = new CreateEmployeeDocumentCommand
                {
                    EmployeeId = !string.IsNullOrEmpty(FilterEmployeeId) && Guid.TryParse(FilterEmployeeId, out var empId) 
                        ? empId 
                        : Guid.Empty,
                    DocumentType = document.DocumentType ?? "Other",
                    Title = document.Title ?? string.Empty,
                    IssueNumber = document.IssueNumber,
                    IssueDate = document.IssueDate,
                    ExpiryDate = document.ExpiryDate,
                    Notes = document.Notes
                };
                await Client.CreateEmployeeDocumentEndpointAsync("1", command);
            },
            updateFunc: async (id, document) =>
            {
                var command = new UpdateEmployeeDocumentCommand
                {
                    Id = id,
                    DocumentType = document.DocumentType,
                    Title = document.Title,
                    IssueNumber = document.IssueNumber,
                    IssueDate = document.IssueDate,
                    ExpiryDate = document.ExpiryDate,
                    Notes = document.Notes
                };
                await Client.UpdateEmployeeDocumentEndpointAsync("1", id, command);
            },
            deleteFunc: async id =>
            {
                await Client.DeleteEmployeeDocumentEndpointAsync("1", id);
            });

        await Task.CompletedTask;
    }

    private void ClearFilter()
    {
        NavigationManager.NavigateTo("/human-resources/employees/documents");
    }

    private async Task ViewDocument(EmployeeDocumentResponse document)
    {
        if (!string.IsNullOrWhiteSpace(document.FilePath))
        {
            // TODO: Implement document viewing when file storage is available
            Snackbar.Add("Document viewing coming soon", Severity.Info);
        }
        else
        {
            Snackbar.Add("No file attached to this document", Severity.Warning);
        }
        await Task.CompletedTask;
    }
}

public class EmployeeDocumentViewModel
{
    public DefaultIdType Id { get; set; }
    public string? DocumentType { get; set; }
    public string? Title { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? IssueNumber { get; set; }
    public DateTime? IssueDate { get; set; }
    public string? Notes { get; set; }
    
    // Advanced search properties
    public DateRange? ExpiryDateRange { get; set; }
    public bool ExpiredOnly { get; set; }
}

