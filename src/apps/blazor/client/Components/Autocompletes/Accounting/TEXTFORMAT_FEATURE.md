# AutocompleteChartOfAccountId - TextFormat Feature

## Summary

Successfully updated `AutocompleteChartOfAccountId` to support customizable text display formats, matching the functionality of `AutocompleteChartOfAccountCode`.

---

## Changes Made

### 1. Added TextFormat Parameter

```csharp
/// <summary>
/// Controls how the selected text is displayed.
/// Accepted values: "Name" (default), "Code", "CodeName", "NameCode".
/// </summary>
[Parameter]
public string TextFormat { get; set; } = "Name";
```

### 2. Updated GetTextValue Method

The method now formats the display text based on the `TextFormat` parameter:

```csharp
protected override string GetTextValue(DefaultIdType? id)
{
    if (id == null || id == default) return string.Empty;

    if (_cache.TryGetValue(id.Value, out var dto))
    {
        var name = dto.Name ?? string.Empty;
        var code = dto.AccountCode ?? string.Empty;
        
        return TextFormat switch
        {
            "Code" => code,
            "CodeName" => string.IsNullOrWhiteSpace(name) ? code : $"{code} - {name}",
            "NameCode" => string.IsNullOrWhiteSpace(name) ? code : $"{name} ({code})",
            _ => name // default: Name
        };
    }

    return string.Empty;
}
```

---

## TextFormat Options

| Value | Display Format | Example |
|-------|---------------|---------|
| `"Name"` | Account Name only (default) | `"Cash in Bank"` |
| `"Code"` | Account Code only | `"1010"` |
| `"CodeName"` | Code - Name | `"1010 - Cash in Bank"` |
| `"NameCode"` | Name (Code) | `"Cash in Bank (1010)"` |

---

## Usage Examples

### Default (Name only)
```razor
<AutocompleteChartOfAccountId @bind-Value="model.AccountId"
                              Label="Account" />
<!-- Displays: "Cash in Bank" -->
```

### Code only
```razor
<AutocompleteChartOfAccountId @bind-Value="model.AccountId"
                              Label="Account"
                              TextFormat="Code" />
<!-- Displays: "1010" -->
```

### Code - Name format
```razor
<AutocompleteChartOfAccountId @bind-Value="model.AccountId"
                              Label="Account"
                              TextFormat="CodeName" />
<!-- Displays: "1010 - Cash in Bank" -->
```

### Name (Code) format
```razor
<AutocompleteChartOfAccountId @bind-Value="model.AccountId"
                              Label="Account"
                              TextFormat="NameCode" />
<!-- Displays: "Cash in Bank (1010)" -->
```

---

## Benefits

1. **Consistency**: Both `AutocompleteChartOfAccountId` and `AutocompleteChartOfAccountCode` now support the same TextFormat options
2. **Flexibility**: Users can choose their preferred display format
3. **User Experience**: Can show more context (code + name) when needed
4. **Backward Compatible**: Default behavior (Name only) remains unchanged

---

## Component Comparison

### AutocompleteChartOfAccountId
- **Value Type**: `DefaultIdType?` (Guid)
- **Returns**: Account ID
- **TextFormat**: ✅ Supported (Name, Code, CodeName, NameCode)
- **Nullable**: ✅ Yes

### AutocompleteChartOfAccountCode
- **Value Type**: `string?`
- **Returns**: Account Code (string)
- **TextFormat**: ✅ Supported (Name, Code, CodeName, NameCode)
- **Nullable**: ✅ Yes

Both components now have feature parity for text formatting!

---

## Usage in Tax Codes

The Tax Codes implementation already uses this feature:

```razor
<AutocompleteChartOfAccountId @bind-Value="context.TaxCollectedAccountId"
                              Label="Tax Collected Account"
                              TextFormat="CodeName"
                              Variant="Variant.Filled" />
```

This displays accounts as "1010 - Cash in Bank" format, providing better context for users.

---

## Status

✅ **Implementation Complete**
- TextFormat parameter added
- GetTextValue method updated
- All format options supported (Name, Code, CodeName, NameCode)
- No breaking changes
- Backward compatible

---

**Date**: November 3, 2025  
**Component**: AutocompleteChartOfAccountId  
**Feature**: TextFormat customization  
**Status**: ✅ Complete

