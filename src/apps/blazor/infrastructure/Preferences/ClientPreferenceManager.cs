using System.Text.RegularExpressions;
using Blazored.LocalStorage;
using FSH.Starter.Blazor.Infrastructure.Themes;
using MudBlazor;

namespace FSH.Starter.Blazor.Infrastructure.Preferences;

public class ClientPreferenceManager : IClientPreferenceManager
{
    private readonly ILocalStorageService _localStorageService;

    public ClientPreferenceManager(
        ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async Task<bool> ToggleDarkModeAsync()
    {
        if (await GetPreference().ConfigureAwait(false) is ClientPreference preference)
        {
            preference.IsDarkMode = !preference.IsDarkMode;
            await SetPreference(preference).ConfigureAwait(false);
            return !preference.IsDarkMode;
        }

        return false;
    }

    public async Task<bool> ToggleDrawerAsync()
    {
        if (await GetPreference().ConfigureAwait(false) is ClientPreference preference)
        {
            preference.IsDrawerOpen = !preference.IsDrawerOpen;
            await SetPreference(preference).ConfigureAwait(false);
            return preference.IsDrawerOpen;
        }

        return false;
    }

    public async Task<bool> ToggleLayoutDirectionAsync()
    {
        if (await GetPreference().ConfigureAwait(false) is ClientPreference preference)
        {
            preference.IsRTL = !preference.IsRTL;
            await SetPreference(preference).ConfigureAwait(false);
            return preference.IsRTL;
        }

        return false;
    }

    public async Task<bool> ChangeLanguageAsync(string languageCode)
    {
        //if (await GetPreference() is ClientPreference preference)
        //{
        //    var language = Array.Find(LocalizationConstants.SupportedLanguages, a => a.Code == languageCode);
        //    if (language?.Code is not null)
        //    {
        //        preference.LanguageCode = language.Code;
        //        preference.IsRTL = language.IsRTL;
        //    }
        //    else
        //    {
        //        preference.LanguageCode = "en-EN";
        //        preference.IsRTL = false;
        //    }

        //    await SetPreference(preference);
        //    return true;
        //}

        return false;
    }

    public async Task<MudTheme> GetCurrentThemeAsync()
    {
        if (await GetPreference().ConfigureAwait(false) is ClientPreference { IsDarkMode: true })
            return new FshTheme();

        return new FshTheme();
    }

    public async Task<string> GetPrimaryColorAsync()
    {
        if (await GetPreference().ConfigureAwait(false) is ClientPreference preference)
        {
            string colorCode = preference.PrimaryColor;
            if (Regex.Match(colorCode, "^#(?:[0-9a-fA-F]{3,4}){1,2}$").Success)
            {
                return colorCode;
            }
            else
            {
                preference.PrimaryColor = CustomColors.Light.Primary;
                await SetPreference(preference).ConfigureAwait(false);
                return preference.PrimaryColor;
            }
        }

        return CustomColors.Light.Primary;
    }

    public async Task<bool> IsRTL()
    {
        if (await GetPreference().ConfigureAwait(false) is ClientPreference preference)
        {
            return preference.IsRTL;
        }

        return false;
    }

    public async Task<bool> IsDrawerOpen()
    {
        if (await GetPreference().ConfigureAwait(false) is ClientPreference preference)
        {
            return preference.IsDrawerOpen;
        }

        return false;
    }

    public static string Preference = "clientPreference";

    public async Task<IPreference> GetPreference()
    {
        return await _localStorageService.GetItemAsync<ClientPreference>(Preference).ConfigureAwait(false) ?? new ClientPreference();
    }

    public async Task SetPreference(IPreference preference)
    {
        await _localStorageService.SetItemAsync(Preference, preference as ClientPreference).ConfigureAwait(false);
    }
}
