namespace FSH.Starter.Blazor.Client.Pages.Identity.Users;

public partial class Audit : ComponentBase
{
    [Inject]
    private IClient Client { get; set; } = default!;
    [CascadingParameter] protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Parameter] public DefaultIdType Id { get; set; }

    protected EntityClientTableContext<AuditTrailExtension, DefaultIdType, object> Context { get; set; } = default!;

    private readonly string? _searchString;
    private string? _subHeader;
    private readonly MudDateRangePicker _dateRangePicker = default!;
    private readonly DateRange? _dateRange;
    private readonly bool _searchInOldValues;
    private readonly bool _searchInNewValues;
    private List<AuditTrailExtension> _trails = [];

    // Configure Automapper
    static Audit() =>
        TypeAdapterConfig<AuditTrail, AuditTrailExtension>.NewConfig().Map(
            dest => dest.UtcTime,
            src => DateTime.SpecifyKind(src.DateTime, DateTimeKind.Utc).ToLocalTime());



    protected override async Task OnInitializedAsync()
    {
        if (Id == DefaultIdType.Empty)
        {
            var state = await AuthState;
            if (state != null)
            {
                Id = new DefaultIdType(state.User.GetUserId()!);
            }
        }
        _subHeader = $"Audit Trail for User {Id}";
        Context = new EntityClientTableContext<AuditTrailExtension, DefaultIdType, object>(
            entityNamePlural: "Trails",
            searchAction: true.ToString(),
            fields:
            [
                new EntityField<AuditTrailExtension>(audit => audit.Id, "Id"),
                new EntityField<AuditTrailExtension>(audit => audit.Entity, "Entity"),
                new EntityField<AuditTrailExtension>(audit => audit.DateTime, "Date", Template: DateFieldTemplate),
                new EntityField<AuditTrailExtension>(audit => audit.Operation, "Operation")
            ],
            loadDataFunc: async () => _trails = (await Client.GetUserAuditTrailEndpointAsync(Id)).Adapt<List<AuditTrailExtension>>(),
            searchFunc: (searchString, trail) =>
                (string.IsNullOrWhiteSpace(searchString) // check Search String
                    || trail.Entity?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true
                    || (_searchInOldValues &&
                        trail.PreviousValues?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true)
                    || (_searchInNewValues &&
                        trail.NewValues?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true))
                && ((_dateRange?.Start is null && _dateRange?.End is null) // check Date Range
                    || (_dateRange?.Start is not null && _dateRange.End is null && trail.DateTime >= _dateRange.Start)
                    || (_dateRange?.Start is null && _dateRange?.End is not null && trail.DateTime <= _dateRange.End + new TimeSpan(0, 11, 59, 59, 999))
                    || (trail.DateTime >= _dateRange!.Start && trail.DateTime <= _dateRange.End + new TimeSpan(0, 11, 59, 59, 999))),
            hasExtraActionsFunc: () => true);
    }

    private void ShowBtnPress(DefaultIdType id)
    {
        var trail = _trails.First(f => f.Id == id);
        trail.ShowDetails = !trail.ShowDetails;
        foreach (var otherTrail in _trails.Except([trail]))
        {
            otherTrail.ShowDetails = false;
        }
    }

    public class AuditTrailExtension : AuditTrail
    {
        public bool ShowDetails { get; set; }
        public DateTime UtcTime { get; set; }
    }
}
