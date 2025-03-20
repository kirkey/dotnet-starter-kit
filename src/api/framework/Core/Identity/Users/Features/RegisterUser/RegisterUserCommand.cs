using System.Text.Json.Serialization;
using MediatR;

namespace FSH.Framework.Core.Identity.Users.Features.RegisterUser;
public class RegisterUserCommand : IRequest<RegisterUserResponse>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
    public string? PhoneNumber { get; set; }

    [JsonIgnore]
    public string? Origin { get; set; }
}
