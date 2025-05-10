namespace FSH.Framework.Infrastructure.Jobs;
public class HangfireOptions
{
    public string UserName { get; set; } = "admin";
    public string Password { get; set; } = "zaneco@HF0";
    public string Route { get; set; } = "/jobs";
}
