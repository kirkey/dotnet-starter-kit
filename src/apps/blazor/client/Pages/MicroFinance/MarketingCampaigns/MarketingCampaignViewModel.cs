namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.MarketingCampaigns;

public class MarketingCampaignViewModel
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? CampaignType { get; set; }
    public DateTimeOffset StartDate { get; set; } = DateTimeOffset.Now;
    public string? Channels { get; set; }
    public decimal Budget { get; set; }
    public int TargetAudience { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? EndDate { get; set; }
}
