namespace FSH.Framework.Core.Identity.Users.Features.ResetPassword;
public class ResetPasswordCommand
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Token { get; set; } = null!;
}
