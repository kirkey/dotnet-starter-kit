using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.FluentUI.AspNetCore.Components;
using Shared.Authorization;

namespace FSH.Starter.Blazor.FluentClient.Pages.Identity.Users;

public partial class UsersComponent : ComponentBase
{
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;
    [Inject]
    protected IApiClient UsersClient { get; set; } = default!;
    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;
    [Inject]
    protected IToastService ToastService { get; set; } = default!;
    [Inject]
    protected IDialogService DialogService { get; set; } = default!;

    protected bool _canExportUsers;
    protected bool _canViewAuditTrails;
    protected bool _canViewRoles;

    // Data properties
    protected IQueryable<UserDetail> _users = Enumerable.Empty<UserDetail>().AsQueryable();
    protected bool _loading = true;
    protected string _searchString = string.Empty;
    protected PaginationState _pagination = new() { ItemsPerPage = 10 };

    // Dialog properties
    protected bool _dialogHidden = true;
    protected UserDetailViewModel _userModel = new();
    protected UserDetail? _editingUser = null;
    protected bool _showPassword = false;
    protected bool _saving = false;

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canExportUsers = await AuthService.HasPermissionAsync(user, FshActions.Export, FshResources.Users);
        _canViewRoles = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.UserRoles);
        _canViewAuditTrails = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.AuditTrails);

        await LoadUsers();
    }

    protected async Task LoadUsers()
    {
        _loading = true;
        try
        {
            var users = await UsersClient.GetUsersListEndpointAsync();
            _users = users.AsQueryable();
        }
        catch (Exception ex)
        {
            ToastService.ShowError($"Failed to load users: {ex.Message}");
        }
        finally
        {
            _loading = false;
            StateHasChanged();
        }
    }

    protected async Task OnSearchChanged()
    {
        await Task.Delay(300); // Debounce
        await ApplySearch();
    }

    protected async Task ApplySearch()
    {
        if (string.IsNullOrWhiteSpace(_searchString))
        {
            await LoadUsers();
            return;
        }

        _loading = true;
        try
        {
            var allUsers = await UsersClient.GetUsersListEndpointAsync();
            _users = allUsers.Where(u =>
                u.FirstName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true ||
                u.LastName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true ||
                u.Email?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true ||
                u.PhoneNumber?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true ||
                u.UserName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true
            ).AsQueryable();
        }
        catch (Exception ex)
        {
            ToastService.ShowError($"Failed to search users: {ex.Message}");
        }
        finally
        {
            _loading = false;
            StateHasChanged();
        }
    }

    protected async Task RefreshData()
    {
        await LoadUsers();
    }

    protected void OpenCreateDialog()
    {
        _editingUser = null;
        _userModel = new UserDetailViewModel();
        _dialogHidden = false;
    }

    protected void EditUser(UserDetail user)
    {
        _editingUser = user;
        _userModel = new UserDetailViewModel
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty
        };
        _dialogHidden = false;
    }

    protected void CloseDialog()
    {
        _dialogHidden = true;
        _editingUser = null;
        _userModel = new UserDetailViewModel();
    }

    protected async Task SaveUser()
    {
        _saving = true;
        try
        {
            if (_editingUser != null)
            {
                // Update user logic here
                ToastService.ShowSuccess("User updated successfully");
            }
            else
            {
                await UsersClient.RegisterUserEndpointAsync(_userModel);
                ToastService.ShowSuccess("User created successfully");
            }

            CloseDialog();
            await LoadUsers();
        }
        catch (Exception ex)
        {
            ToastService.ShowError($"Failed to save user: {ex.Message}");
        }
        finally
        {
            _saving = false;
        }
    }

    protected async Task DeleteUser(UserDetail user)
    {
        var confirmed = await DialogService.ShowConfirmationAsync(
            $"Delete User",
            $"Are you sure you want to delete user '{user.UserName}'?",
            "Yes",
            "No");

        if (confirmed)
        {
            try
            {
                // Delete user logic here
                ToastService.ShowSuccess("User deleted successfully");
                await LoadUsers();
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Failed to delete user: {ex.Message}");
            }
        }
    }

    protected void ViewProfile(DefaultIdType userId) =>
        Navigation.NavigateTo($"/identity/users/{userId}/profile");

    protected void ManageRoles(DefaultIdType userId) =>
        Navigation.NavigateTo($"/identity/users/{userId}/roles");

    protected void ViewAuditTrails(DefaultIdType userId) =>
        Navigation.NavigateTo($"/identity/users/{userId}/audit-trail");

    protected void TogglePasswordVisibility()
    {
        _showPassword = !_showPassword;
    }

    public class UserDetailViewModel : RegisterUserCommand
    {
        public DefaultIdType Id { get; set; } = default!;
    }
}
