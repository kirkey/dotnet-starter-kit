using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorApp1.Services;

public class ThemeService
{
    private DesignThemeModes _currentMode = DesignThemeModes.Light;
    private OfficeColor _currentColor = OfficeColor.Default;
    
    public event Action<DesignThemeModes, OfficeColor>? ThemeChanged;
    
    public DesignThemeModes CurrentMode => _currentMode;
    public OfficeColor CurrentColor => _currentColor;
    
    public void SetThemeMode(DesignThemeModes mode)
    {
        if (_currentMode != mode)
        {
            _currentMode = mode;
            ThemeChanged?.Invoke(_currentMode, _currentColor);
        }
    }
    
    public void SetThemeColor(OfficeColor color)
    {
        if (_currentColor != color)
        {
            _currentColor = color;
            ThemeChanged?.Invoke(_currentMode, _currentColor);
        }
    }
    
    public void SetTheme(DesignThemeModes mode, OfficeColor color)
    {
        if (_currentMode != mode || _currentColor != color)
        {
            _currentMode = mode;
            _currentColor = color;
            ThemeChanged?.Invoke(_currentMode, _currentColor);
        }
    }
}
