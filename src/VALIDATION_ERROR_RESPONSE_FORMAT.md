# Validation Error Response Format - Enhanced

## Overview
The API now returns detailed validation errors with property names and specific error messages when validation fails.

---

## ✅ **Enhanced Error Response Format**

### What Changed
Previously, validation errors were returned as a flat list of messages without property context. Now errors are grouped by property name for better clarity.

### New Response Structure

When a validation error occurs (HTTP 400 Bad Request), the response will look like:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Validation Error",
  "status": 400,
  "detail": "One or more validation errors occurred",
  "instance": "/api/v1/accounting/journal-entries",
  "errors": {
    "PropertyName1": [
      "Error message 1",
      "Error message 2"
    ],
    "PropertyName2": [
      "Error message 1"
    ]
  }
}
```

---

## 📋 **Examples**

### Example 1: Missing Required Fields

**Request:**
```json
POST /api/v1/accounting/journal-entries
{
  "date": "2025-11-03T00:00:00Z",
  "referenceNumber": "",
  "source": "",
  "description": ""
}
```

**Response (400 Bad Request):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Validation Error",
  "status": 400,
  "detail": "One or more validation errors occurred",
  "instance": "/api/v1/accounting/journal-entries",
  "errors": {
    "ReferenceNumber": [
      "Reference number is required."
    ],
    "Source": [
      "Source is required."
    ],
    "Description": [
      "Description is required."
    ],
    "Lines": [
      "Lines are required."
    ]
  }
}
```

---

### Example 2: Not Enough Lines

**Request:**
```json
POST /api/v1/accounting/journal-entries
{
  "date": "2025-11-03T00:00:00Z",
  "referenceNumber": "JE-001",
  "source": "ManualEntry",
  "description": "Test",
  "lines": [
    {
      "accountId": "guid-1",
      "debitAmount": 100,
      "creditAmount": 0
    }
  ]
}
```

**Response (400 Bad Request):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Validation Error",
  "status": 400,
  "detail": "One or more validation errors occurred",
  "instance": "/api/v1/accounting/journal-entries",
  "errors": {
    "Lines": [
      "At least 2 lines are required for a balanced journal entry."
    ]
  }
}
```

---

### Example 3: Unbalanced Entry

**Request:**
```json
POST /api/v1/accounting/journal-entries
{
  "date": "2025-11-03T00:00:00Z",
  "referenceNumber": "JE-001",
  "source": "ManualEntry",
  "description": "Test",
  "lines": [
    {
      "accountId": "guid-1",
      "debitAmount": 100,
      "creditAmount": 0
    },
    {
      "accountId": "guid-2",
      "debitAmount": 0,
      "creditAmount": 50
    }
  ]
}
```

**Response (400 Bad Request):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Validation Error",
  "status": 400,
  "detail": "One or more validation errors occurred",
  "instance": "/api/v1/accounting/journal-entries",
  "errors": {
    "Lines": [
      "The journal entry must be balanced (total debits must equal total credits)."
    ]
  }
}
```

---

### Example 4: Line-Level Validation Errors

**Request:**
```json
POST /api/v1/accounting/journal-entries
{
  "date": "2025-11-03T00:00:00Z",
  "referenceNumber": "JE-001",
  "source": "ManualEntry",
  "description": "Test",
  "lines": [
    {
      "accountId": "00000000-0000-0000-0000-000000000000",
      "debitAmount": 100,
      "creditAmount": 50
    },
    {
      "accountId": "guid-2",
      "debitAmount": 0,
      "creditAmount": 0
    }
  ]
}
```

**Response (400 Bad Request):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Validation Error",
  "status": 400,
  "detail": "One or more validation errors occurred",
  "instance": "/api/v1/accounting/journal-entries",
  "errors": {
    "Lines[0].AccountId": [
      "Account ID is required for each line."
    ],
    "Lines[0]": [
      "A line cannot have both debit and credit amounts."
    ],
    "Lines[1]": [
      "Each line must have either a debit or credit amount."
    ]
  }
}
```

---

### Example 5: String Length Validation

**Request:**
```json
POST /api/v1/accounting/journal-entries
{
  "date": "2025-11-03T00:00:00Z",
  "referenceNumber": "This is a very long reference number that exceeds the maximum allowed length",
  "source": "ManualEntry",
  "description": "Test",
  "lines": [
    {
      "accountId": "guid-1",
      "debitAmount": 100,
      "creditAmount": 0,
      "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. This text is way too long and exceeds the 500 character limit for line descriptions."
    },
    {
      "accountId": "guid-2",
      "debitAmount": 0,
      "creditAmount": 100
    }
  ]
}
```

**Response (400 Bad Request):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Validation Error",
  "status": 400,
  "detail": "One or more validation errors occurred",
  "instance": "/api/v1/accounting/journal-entries",
  "errors": {
    "ReferenceNumber": [
      "Reference number cannot exceed 32 characters."
    ],
    "Lines[0].Description": [
      "Line description cannot exceed 500 characters."
    ]
  }
}
```

---

### Example 6: Multiple Errors on Same Property

**Request:**
```json
POST /api/v1/accounting/journal-entries
{
  "date": "2025-11-03T00:00:00Z",
  "referenceNumber": "",
  "source": "ManualEntry",
  "description": "Test",
  "lines": []
}
```

**Response (400 Bad Request):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Validation Error",
  "status": 400,
  "detail": "One or more validation errors occurred",
  "instance": "/api/v1/accounting/journal-entries",
  "errors": {
    "ReferenceNumber": [
      "Reference number is required."
    ],
    "Lines": [
      "Lines are required.",
      "At least 2 lines are required for a balanced journal entry."
    ]
  }
}
```

---

## 🔍 **Understanding the Error Structure**

### Response Properties

| Property | Type | Description |
|----------|------|-------------|
| **type** | string | URI reference that identifies the problem type |
| **title** | string | Short, human-readable summary ("Validation Error") |
| **status** | number | HTTP status code (400 for validation errors) |
| **detail** | string | Human-readable explanation of the problem |
| **instance** | string | URI reference that identifies the specific occurrence |
| **errors** | object | Dictionary of validation errors grouped by property |

### Error Dictionary Structure

The `errors` object is a dictionary where:
- **Key**: Property name (e.g., "ReferenceNumber", "Lines[0].AccountId")
- **Value**: Array of error messages for that property

### Property Name Patterns

- **Top-level properties**: `"PropertyName"`
- **Collection items**: `"Lines[0]"`, `"Lines[1]"`
- **Nested properties**: `"Lines[0].AccountId"`, `"Lines[0].Description"`

---

## 💡 **Best Practices for Handling Validation Errors**

### Client-Side Handling

#### JavaScript/TypeScript Example:
```typescript
try {
  const response = await fetch('/api/v1/accounting/journal-entries', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(journalEntry)
  });

  if (!response.ok) {
    const errorData = await response.json();
    
    if (errorData.errors) {
      // Display errors grouped by field
      Object.keys(errorData.errors).forEach(field => {
        const messages = errorData.errors[field];
        console.error(`${field}: ${messages.join(', ')}`);
        
        // Display in UI
        showFieldError(field, messages);
      });
    } else {
      console.error(errorData.detail);
    }
  }
} catch (error) {
  console.error('Request failed:', error);
}
```

#### C# Blazor Example:
```csharp
try 
{
    await Client.JournalEntryCreateEndpointAsync("1", command);
    Snackbar.Add("Success!", Severity.Success);
}
catch (ApiException ex) when (ex.StatusCode == 400)
{
    var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(ex.Response);
    
    if (problemDetails?.Extensions?.TryGetValue("errors", out var errorsObj) == true)
    {
        var errors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(
            errorsObj.ToString());
        
        foreach (var (field, messages) in errors)
        {
            foreach (var message in messages)
            {
                Snackbar.Add($"{field}: {message}", Severity.Error);
            }
        }
    }
}
```

---

## 📊 **Common Validation Error Scenarios**

### Journal Entry Validation

| Scenario | Property | Error Message |
|----------|----------|---------------|
| Missing date | Date | "Date is required." |
| Missing reference | ReferenceNumber | "Reference number is required." |
| Reference too long | ReferenceNumber | "Reference number cannot exceed 32 characters." |
| Missing source | Source | "Source is required." |
| Source too long | Source | "Source cannot exceed 64 characters." |
| Missing description | Description | "Description is required." |
| Description too long | Description | "Description cannot exceed 1000 characters." |
| Negative original amount | OriginalAmount | "Original amount must be non-negative." |
| Lines is null | Lines | "Lines are required." |
| Not enough lines | Lines | "At least 2 lines are required for a balanced journal entry." |
| Unbalanced entry | Lines | "The journal entry must be balanced (total debits must equal total credits)." |

### Journal Entry Line Validation

| Scenario | Property | Error Message |
|----------|----------|---------------|
| Missing account | Lines[n].AccountId | "Account ID is required for each line." |
| Negative debit | Lines[n].DebitAmount | "Debit amount must be non-negative." |
| Negative credit | Lines[n].CreditAmount | "Credit amount must be non-negative." |
| Both debit and credit | Lines[n] | "A line cannot have both debit and credit amounts." |
| Neither debit nor credit | Lines[n] | "Each line must have either a debit or credit amount." |
| Description too long | Lines[n].Description | "Line description cannot exceed 500 characters." |
| Reference too long | Lines[n].Reference | "Line reference cannot exceed 100 characters." |

---

## 🎯 **Benefits of the Enhanced Format**

1. ✅ **Clear Property Identification**: Know exactly which field has an error
2. ✅ **Multiple Errors Per Field**: See all validation issues for a property
3. ✅ **Array Index Tracking**: Identify which line item has errors
4. ✅ **Nested Property Support**: Validate complex object graphs
5. ✅ **Structured Format**: Easy to parse and display in UI
6. ✅ **Standard RFC 7807**: Follows Problem Details specification

---

## 🔧 **Technical Implementation**

### CustomExceptionHandler Changes

The `CustomExceptionHandler` now groups validation errors by property name:

```csharp
case FluentValidation.ValidationException fluentException:
{
    problemDetails.Detail = "One or more validation errors occurred";
    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
    problemDetails.Title = "Validation Error";
    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
    
    // Group errors by property name for better readability
    var errorsDictionary = fluentException.Errors
        .GroupBy(x => x.PropertyName)
        .ToDictionary(
            g => g.Key,
            g => g.Select(x => x.ErrorMessage).ToArray()
        );
    
    problemDetails.Extensions.Add("errors", errorsDictionary);
    break;
}
```

### Before vs After

**Before:**
```json
{
  "errors": [
    "Reference number is required.",
    "Description is required.",
    "Lines are required."
  ]
}
```

**After:**
```json
{
  "errors": {
    "ReferenceNumber": ["Reference number is required."],
    "Description": ["Description is required."],
    "Lines": ["Lines are required."]
  }
}
```

---

## 📝 **Testing the Enhanced Format**

### Using curl:
```bash
curl -X POST https://localhost:7000/api/v1/accounting/journal-entries \
  -H "Content-Type: application/json" \
  -d '{
    "date": "2025-11-03T00:00:00Z",
    "referenceNumber": "",
    "source": "",
    "description": ""
  }' | jq .
```

### Expected Response:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Validation Error",
  "status": 400,
  "detail": "One or more validation errors occurred",
  "instance": "/api/v1/accounting/journal-entries",
  "errors": {
    "ReferenceNumber": ["Reference number is required."],
    "Source": ["Source is required."],
    "Description": ["Description is required."],
    "Lines": ["Lines are required."]
  }
}
```

---

## 🎓 **Summary**

The enhanced validation error response format provides:

- ✅ **Property-specific errors** with clear field names
- ✅ **Array indexing** for collection validation errors
- ✅ **Multiple errors per property** support
- ✅ **Structured, parseable format** for UI display
- ✅ **RFC 7807 compliance** for standard problem details
- ✅ **Better developer experience** with detailed error information

Now when you receive "one or more validation errors occurred", you'll also get the complete details about which fields failed validation and why!

---

**Last Updated:** November 3, 2025  
**Status:** ✅ Implemented and Ready to Use

