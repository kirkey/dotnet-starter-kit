using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Infrastructure.Api;
using Mapster;
using Microsoft.AspNetCore.Components;
using Shared.Authorization;

namespace FSH.Starter.Blazor.Client.Pages.App;

public partial class Groups
{
    [Inject] protected IApiClient ApiClient { get; set; } = default!;

    protected EntityServerTableContext<GroupDto, DefaultIdType, GroupViewModel> Context { get; set; } = default!;

    private EntityTable<GroupDto, DefaultIdType, GroupViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new EntityServerTableContext<GroupDto, DefaultIdType, GroupViewModel>(
            entityName: "Group",
            entityNamePlural: "Groups",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<GroupDto>(dto => dto.Application, "Application", "Application"),
                new EntityField<GroupDto>(dto => dto.Parent, "Parent", "Parent"),
                new EntityField<GroupDto>(dto => dto.Code, "Code", "Code"),
                new EntityField<GroupDto>(dto => dto.Name, "Name", "Name"),
                new EntityField<GroupDto>(dto => dto.Amount, "Amount", "Amount"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                // var paginationFilter = filter.Adapt<PaginationFilter>();
                // var dtoPagedList = await ApiClient.GroupGetListEndpointAsync("1", paginationFilter);
                
                var groupSearchCommand = filter.Adapt<GroupSearchCommand>();
                var dtoPagedList = await ApiClient.GroupSearchEndpointAsync("1", groupSearchCommand);
                
                return dtoPagedList.Adapt<PaginationResponse<GroupDto>>();
            },
            createFunc: async model =>
            {
                await ApiClient.GroupCreateEndpointAsync("1", model.Adapt<GroupCreateCommand>());
            },
            updateFunc: async (id, model) =>
            {
                await ApiClient.GroupUpdateEndpointAsync("1", id, model.Adapt<GroupUpdateCommand>());
            },
            deleteFunc: async id => await ApiClient.GroupDeleteEndpointAsync("1", id));
}

public partial class GroupViewModel : GroupUpdateCommand
{
}
