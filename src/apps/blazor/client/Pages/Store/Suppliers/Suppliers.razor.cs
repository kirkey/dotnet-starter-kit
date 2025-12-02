namespace FSH.Starter.Blazor.Client.Pages.Store.Suppliers;

/// <summary>
/// Suppliers page logic. Provides CRUD and server-side search over Supplier entities via the generated API client.
/// Mirrors Budgets/Items patterns for consistent UX. Supports workflow actions for activation/deactivation.
/// </summary>
public partial class Suppliers
{
    [Inject] protected ImageUrlService ImageUrlService { get; set; } = null!;
    

    protected EntityServerTableContext<SupplierResponse, DefaultIdType, SupplierViewModel> Context { get; set; } = null!;

    private ClientPreference _preference = new();
    private EntityTable<SupplierResponse, DefaultIdType, SupplierViewModel> _table = null!;

    protected override async Task OnInitializedAsync()
    {
        // Load preference
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        // Subscribe to preference changes
        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        Context = new EntityServerTableContext<SupplierResponse, DefaultIdType, SupplierViewModel>(
            entityName: "Supplier",
            entityNamePlural: "Suppliers",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<SupplierResponse>(response => response.ImageUrl, "Image", "ImageUrl", Template: TemplateImage),
                new EntityField<SupplierResponse>(response => response.Code, "Code", "Code"),
                new EntityField<SupplierResponse>(response => response.Name, "Name", "Name"),
                new EntityField<SupplierResponse>(response => response.ContactPerson, "Contact", "ContactPerson"),
                new EntityField<SupplierResponse>(response => response.Email, "Email", "Email"),
                new EntityField<SupplierResponse>(response => response.Phone, "Phone", "Phone"),
                new EntityField<SupplierResponse>(response => response.IsActive, "Active", "IsActive", typeof(bool)),
                new EntityField<SupplierResponse>(response => response.Rating, "Rating", "Rating", typeof(decimal))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id ?? DefaultIdType.Empty,
            searchFunc: async filter =>
            {
                var command = new SearchSuppliersCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchSuppliersEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<SupplierResponse>>();
            },
            createFunc: async viewModel =>
            {
                var command = viewModel.Adapt<CreateSupplierCommand>();
                command.Code = command.Code!.ToUpperInvariant();
                command.Name = command.Name!.ToUpperInvariant();
                if (viewModel.Image != null)
                {
                    command.Image = new FileUploadCommand
                    {
                        Name = viewModel.Image.Name,
                        Extension = viewModel.Image.Extension,
                        Data = viewModel.Image.Data,
                        Size = viewModel.Image.Size,
                    };
                }
                await Client.CreateSupplierEndpointAsync("1", command).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                // Create a new command with uppercase Code and Name, and formatted Image
                var command = new UpdateSupplierCommand
                {
                    Id = id,
                    Code = viewModel.Code?.ToUpperInvariant() ?? string.Empty,
                    Name = viewModel.Name?.ToUpperInvariant() ?? string.Empty,
                    Description = viewModel.Description,
                    ContactPerson = viewModel.ContactPerson ?? string.Empty,
                    Email = viewModel.Email ?? string.Empty,
                    Phone = viewModel.Phone ?? string.Empty,
                    Address = viewModel.Address ?? string.Empty,
                    PostalCode = viewModel.PostalCode,
                    Website = viewModel.Website,
                    CreditLimit = viewModel.CreditLimit,
                    PaymentTermsDays = viewModel.PaymentTermsDays,
                    IsActive = viewModel.IsActive,
                    Rating = viewModel.Rating,
                    Notes = viewModel.Notes,
                    ImageUrl = viewModel.ImageUrl,
                    Image = viewModel.Image != null
                        ? new FileUploadCommand
                        {
                            Name = viewModel.Image.Name,
                            Extension = viewModel.Image.Extension,
                            Data = viewModel.Image.Data,
                            Size = viewModel.Image.Size,
                        }
                        : null
                };
                await Client.UpdateSupplierEndpointAsync("1", id, command).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteSupplierEndpointAsync("1", id).ConfigureAwait(false));
    }

    /// <summary>
    /// Activates a deactivated supplier.
    /// </summary>
    private async Task ActivateSupplier(DefaultIdType id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Confirm Activation",
            "Are you sure you want to activate this supplier?",
            yesText: "Activate",
            cancelText: "Cancel");

        if (result == true)
        {
            try
            {
                var command = new ActivateSupplierCommand();
                await Client.ActivateSupplierEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Supplier activated successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to activate supplier: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Deactivates an active supplier.
    /// </summary>
    private async Task DeactivateSupplier(DefaultIdType id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Confirm Deactivation",
            "Are you sure you want to deactivate this supplier? Active purchase orders may be affected.",
            yesText: "Deactivate",
            cancelText: "Cancel");

        if (result == true)
        {
            try
            {
                var command = new DeactivateSupplierCommand();
                await Client.DeactivateSupplierEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Supplier deactivated successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to deactivate supplier: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Show suppliers help dialog.
    /// </summary>
    private async Task ShowSuppliersHelp()
    {
        await DialogService.ShowAsync<SuppliersHelpDialog>("Suppliers Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

