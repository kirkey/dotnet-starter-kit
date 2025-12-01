namespace FSH.Starter.Blazor.Infrastructure.Preferences;

public class ClientPreference : IPreference, INotificationMessage
{
    public bool IsDarkMode { get; set; } = true;
    public bool IsRtl { get; set; }
    public bool IsDrawerOpen { get; set; }
    public string PrimaryColor { get; set; } = CustomColors.Light.Primary;
    public string SecondaryColor { get; set; } = CustomColors.Light.Secondary;
    public double BorderRadius { get; set; } = 5;
    public int Elevation { get; set; } = 1;
    public FshTablePreference TablePreference { get; set; } = new();

    /// <summary>
    /// Helper method to extract elevation from preference for notification subscribers.
    /// </summary>
    public static int SetClientPreference(ClientPreference clientPreference)
    {
        return clientPreference.Elevation;
    }

    /// <summary>
    /// Helper method to extract border radius from preference for notification subscribers.
    /// </summary>
    public static double SetClientBorderRadius(ClientPreference clientPreference)
    {
        return clientPreference.BorderRadius;
    }
}
