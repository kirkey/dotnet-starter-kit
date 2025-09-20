namespace FSH.Starter.Blazor.Client.Pages.Todos;

public partial class Todos
{
    [Inject]
    protected IClient ApiClient { get; set; } = default!;

    protected EntityServerTableContext<GetTodoResponse, DefaultIdType, TodoViewModel> Context { get; set; } = default!;

    private EntityTable<GetTodoResponse, DefaultIdType, TodoViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new EntityServerTableContext<GetTodoResponse, DefaultIdType, TodoViewModel>(
            entityName: "Todos",
            entityNamePlural: "Todos",
            entityResource: FshResources.Todos,
            fields:
            [
                new EntityField<GetTodoResponse>(prod => prod.Id, "Id", "Id"),
                new EntityField<GetTodoResponse>(prod => prod.Name, "Name", "Name"),
                new EntityField<GetTodoResponse>(prod => prod.Description, "Description", "Description"),
                new EntityField<GetTodoResponse>(prod => prod.Notes, "Notes", "Notes")
            ],
            enableAdvancedSearch: false,
            idFunc: prod => prod.Id!.Value,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();

                var result = await ApiClient.GetTodoListEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<GetTodoResponse>>();
            },
            createFunc: async todo =>
            {
                await ApiClient.CreateTodoEndpointAsync("1", todo.Adapt<CreateTodoCommand>());
            },
            updateFunc: async (id, todo) =>
            {
                await ApiClient.UpdateTodoEndpointAsync("1", id, todo.Adapt<UpdateTodoCommand>());
            },
            deleteFunc: async id => await ApiClient.DeleteTodoEndpointAsync("1", id));
}

public partial class TodoViewModel : UpdateTodoCommand;
