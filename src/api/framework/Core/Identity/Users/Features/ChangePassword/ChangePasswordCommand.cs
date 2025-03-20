namespace FSH.Framework.Core.Identity.Users.Features.ChangePassword;
public class ChangePasswordCommand
{
    public string Password { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    public string ConfirmNewPassword { get; set; } = null!;
}
