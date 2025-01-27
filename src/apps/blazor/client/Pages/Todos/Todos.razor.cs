﻿using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Infrastructure.Api;
using Mapster;
using Microsoft.AspNetCore.Components;
using Shared.Authorization;

namespace FSH.Starter.Blazor.Client.Pages.Todos;

public partial class Todos
{
    [Inject]
    protected IApiClient ApiClient { get; set; } = default!;

    protected EntityServerTableContext<GetTodoResponse, Guid, TodoViewModel> Context { get; set; } = default!;

    private EntityTable<GetTodoResponse, Guid, TodoViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new EntityServerTableContext<GetTodoResponse, Guid, TodoViewModel>(
            entityName: "Todos",
            entityNamePlural: "Todos",
            entityResource: FshResources.Todos,
            fields: new List<EntityField<GetTodoResponse>>
            {
                new(prod => prod.Id,"Id", "Id"),
                new(prod => prod.Title,"Title", "Title"),
                new(prod => prod.Note, "Note", "Note")
            },
            enableAdvancedSearch: false,
            idFunc: prod => prod.Id!.Value,
            searchFunc: async filter =>
            {
                var todoFilter = filter.Adapt<PaginationFilter>();

                var result = await ApiClient.GetTodoListEndpointAsync("1", todoFilter).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<GetTodoResponse>>();
            },
            createFunc: async todo =>
            {
                await ApiClient.CreateTodoEndpointAsync("1", todo.Adapt<CreateTodoCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, todo) =>
            {
                await ApiClient.UpdateTodoEndpointAsync("1", id, todo.Adapt<UpdateTodoCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await ApiClient.DeleteTodoEndpointAsync("1", id).ConfigureAwait(false));
}

public class TodoViewModel : UpdateTodoCommand
{
}
