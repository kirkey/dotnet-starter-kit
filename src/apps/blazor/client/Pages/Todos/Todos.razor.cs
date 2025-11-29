﻿namespace FSH.Starter.Blazor.Client.Pages.Todos;

/// <summary>
/// Todos page component for managing todo items.
/// </summary>
public partial class Todos
{
    [Inject] protected ICourier Courier { get; set; } = null!;

    /// <summary>
    /// The entity table context for managing todos with server-side operations.
    /// </summary>
    protected EntityServerTableContext<GetTodoResponse, DefaultIdType, TodoViewModel> Context { get; set; } = null!;

    /// <summary>
    /// Reference to the EntityTable component for todos.
    /// </summary>
    private EntityTable<GetTodoResponse, DefaultIdType, TodoViewModel> _table = null!;
    
    private ClientPreference _preference = new();

    /// <summary>
    /// Initializes the component and sets up the entity table context with CRUD operations.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        // Load initial preference from localStorage
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        // Subscribe to elevation changes
        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        // Subscribe to border radius changes
        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        SetupTodosContext();
    }

    /// <summary>
    /// Sets up the entity table context.
    /// </summary>
    private void SetupTodosContext() =>
        Context = new EntityServerTableContext<GetTodoResponse, DefaultIdType, TodoViewModel>(
            entityName: "Todos",
            entityNamePlural: "Todos",
            entityResource: FshResources.Todos,
            fields:
            [
                new EntityField<GetTodoResponse>(response => response.Id, "Id", "Id"),
                new EntityField<GetTodoResponse>(response => response.Name, "Name", "Name"),
                new EntityField<GetTodoResponse>(response => response.Description, "Description", "Description"),
                new EntityField<GetTodoResponse>(response => response.Notes, "Notes", "Notes")
            ],
            enableAdvancedSearch: false,
            idFunc: prod => prod.Id!.Value,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();

                var result = await Client.GetTodoListEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<GetTodoResponse>>();
            },
            createFunc: async todo =>
            {
                await Client.CreateTodoEndpointAsync("1", todo.Adapt<CreateTodoCommand>());
            },
            updateFunc: async (id, todo) =>
            {
                await Client.UpdateTodoEndpointAsync("1", id, todo.Adapt<UpdateTodoCommand>());
            },
            deleteFunc: async id => await Client.DeleteTodoEndpointAsync("1", id),
            exportFunc: async filter =>
            {
                var exportQuery = new ExportTodosQuery
                {
                    Filter = new TodoExportFilter
                    {
                        SearchTerm = filter.Keyword
                    },
                    SheetName = "Todos"
                };
                
                var result = await Client.ExportTodosEndpointAsync("1", exportQuery).ConfigureAwait(false);
                var stream = new MemoryStream(result.Data);
                return new Components.EntityTable.FileResponse(stream);
            },
            importFunc: async fileUpload =>
            {
                var command = new ImportTodosCommand
                {
                    File = fileUpload,
                    SheetName = "Sheet1",
                    ValidateStructure = true
                };
                
                var result = await Client.ImportTodosEndpointAsync("1", command).ConfigureAwait(false);
                return result;
            });

    private async Task ShowTodosHelp()
    {
        await DialogService.ShowAsync<TodosHelpDialog>("Todos Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

/// <summary>
/// View model for todo operations, extending the update command.
/// </summary>
public partial class TodoViewModel : UpdateTodoCommand;
