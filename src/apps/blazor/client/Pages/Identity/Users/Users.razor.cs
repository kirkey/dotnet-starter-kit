namespace FSH.Starter.Blazor.Client.Pages.Identity.Users;

public partial class Users : ComponentBase
{
    [CascadingParameter] protected Task<AuthenticationState> AuthState { get; set; } = null!;
    [Inject] protected IAuthorizationService AuthService { get; set; } = null!;

    [Inject] protected IClient UsersClient { get; set; } = null!;

    protected EntityClientTableContext<UserDetail, DefaultIdType, UserDetailViewModel> Context { get; set; } = null!;

    private bool _canExportUsers;
    private bool _canViewAuditTrails;
    private bool _canViewRoles;

    // Fields for editform
    protected string Password { get; set; } = string.Empty;
    protected string ConfirmPassword { get; set; } = string.Empty;

    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canExportUsers = await AuthService.HasPermissionAsync(user, FshActions.Export, FshResources.Users);
        _canViewRoles = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.UserRoles);
        _canViewAuditTrails = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.AuditTrails);

        Context = new EntityClientTableContext<UserDetail, DefaultIdType, UserDetailViewModel>(
            entityName: "User",
            entityNamePlural: "Users",
            entityResource: FshResources.Users,
            searchAction: FshActions.View,
            updateAction: string.Empty,
            deleteAction: string.Empty,
            fields:
            [
                new EntityField<UserDetail>(userDetail => userDetail.FirstName, "First Name"),
                new EntityField<UserDetail>(userDetail => userDetail.LastName, "Last Name"),
                new EntityField<UserDetail>(userDetail => userDetail.UserName, "UserName"),
                new EntityField<UserDetail>(userDetail => userDetail.Email, "Email"),
                new EntityField<UserDetail>(userDetail => userDetail.PhoneNumber, "PhoneNumber"),
                new EntityField<UserDetail>(userDetail => userDetail.EmailConfirmed, "Email Confirmation", Type: typeof(bool)),
                new EntityField<UserDetail>(userDetail => userDetail.IsActive, "Active", Type: typeof(bool))
            ],
            idFunc: userDetail => userDetail.Id,
            loadDataFunc: async () => [.. (await UsersClient.GetUsersListEndpointAsync())],
            searchFunc: (searchString, userDetail) =>
                string.IsNullOrWhiteSpace(searchString)
                    || userDetail.FirstName?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true
                    || userDetail.LastName?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true
                    || userDetail.Email?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true
                    || userDetail.PhoneNumber?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true
                    || userDetail.UserName?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true,
            createFunc: userDetailExtension => UsersClient.RegisterUserEndpointAsync(userDetailExtension),
            hasExtraActionsFunc: () => true,
            exportAction: string.Empty);
    }

    private void ViewProfile(in DefaultIdType userId) =>
        Navigation.NavigateTo($"/identity/users/{userId}/profile");

    private void ManageRoles(in DefaultIdType userId) =>
        Navigation.NavigateTo($"/identity/users/{userId}/roles");
    private void ViewAuditTrails(in DefaultIdType userId) =>
        Navigation.NavigateTo($"/identity/users/{userId}/audit-trail");

    private void TogglePasswordVisibility()
    {
        if (_passwordVisibility)
        {
            _passwordVisibility = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInput = InputType.Password;
        }
        else
        {
            _passwordVisibility = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInput = InputType.Text;
        }

        Context.AddEditModal.ForceRender();
    }

    public class UserDetailViewModel : RegisterUserCommand
    {
        public DefaultIdType Id { get; set; } = DefaultIdType.Empty!;
    }
}
