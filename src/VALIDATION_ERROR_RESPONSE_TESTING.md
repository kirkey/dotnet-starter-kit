# Testing Validation Error Response Fix

## Issue Fixed
The API was returning "Bad Request Status: 400 Response:" without any content body.

## Changes Made
Updated `CustomExceptionHandler.cs` to properly set:
1. ✅ `problemDetails.Status` property
2. ✅ `httpContext.Response.ContentType = "application/problem+json"`
3. ✅ `problemDetails.Title` property

This ensures the response body is properly serialized and sent to the client.

---

## How to Test

### Test 1: Missing Required Fields

**Request:**
```bash
curl -X POST https://localhost:7000/api/v1/accounting/journal-entries \
  -H "Content-Type: application/json" \
  -H "Accept: application/json" \
  -d '{
    "date": "2025-11-03T00:00:00Z",
    "referenceNumber": "",
    "source": "",
    "description": ""
  }' \
  -w "\nHTTP Status: %{http_code}\n" \
  -v
```

**Expected Response (400 Bad Request):**
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

### Test 2: Lines Missing

**Request:**
```bash
curl -X POST https://localhost:7000/api/v1/accounting/journal-entries \
  -H "Content-Type: application/json" \
  -H "Accept: application/json" \
  -d '{
    "date": "2025-11-03T00:00:00Z",
    "referenceNumber": "JE-001",
    "source": "ManualEntry",
    "description": "Test entry"
  }' \
  -w "\nHTTP Status: %{http_code}\n"
```

**Expected Response:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Validation Error",
  "status": 400,
  "detail": "One or more validation errors occurred",
  "instance": "/api/v1/accounting/journal-entries",
  "errors": {
    "Lines": [
      "Lines are required."
    ]
  }
}
```

---

### Test 3: Not Enough Lines (Only 1 Line)

**Request:**
```bash
curl -X POST https://localhost:7000/api/v1/accounting/journal-entries \
  -H "Content-Type: application/json" \
  -H "Accept: application/json" \
  -d '{
    "date": "2025-11-03T00:00:00Z",
    "referenceNumber": "JE-001",
    "source": "ManualEntry",
    "description": "Test entry",
    "lines": [
      {
        "accountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "debitAmount": 100,
        "creditAmount": 0
      }
    ]
  }' \
  -w "\nHTTP Status: %{http_code}\n"
```

**Expected Response:**
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

### Test 4: Unbalanced Entry

**Request:**
```bash
curl -X POST https://localhost:7000/api/v1/accounting/journal-entries \
  -H "Content-Type: application/json" \
  -H "Accept: application/json" \
  -d '{
    "date": "2025-11-03T00:00:00Z",
    "referenceNumber": "JE-001",
    "source": "ManualEntry",
    "description": "Test entry",
    "lines": [
      {
        "accountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "debitAmount": 100,
        "creditAmount": 0
      },
      {
        "accountId": "4fa85f64-5717-4562-b3fc-2c963f66afa7",
        "debitAmount": 0,
        "creditAmount": 50
      }
    ]
  }' \
  -w "\nHTTP Status: %{http_code}\n"
```

**Expected Response:**
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

### Test 5: Valid Request (Should Succeed)

**Request:**
```bash
curl -X POST https://localhost:7000/api/v1/accounting/journal-entries \
  -H "Content-Type: application/json" \
  -H "Accept: application/json" \
  -d '{
    "date": "2025-11-03T00:00:00Z",
    "referenceNumber": "JE-2025-001",
    "source": "ManualEntry",
    "description": "Test balanced entry",
    "lines": [
      {
        "accountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "debitAmount": 100,
        "creditAmount": 0,
        "description": "Test debit"
      },
      {
        "accountId": "4fa85f64-5717-4562-b3fc-2c963f66afa7",
        "debitAmount": 0,
        "creditAmount": 100,
        "description": "Test credit"
      }
    ]
  }' \
  -w "\nHTTP Status: %{http_code}\n"
```

**Expected Response (201 Created or 200 OK):**
```json
{
  "id": "some-guid"
}
```

---

## Testing from Blazor Client

If testing from the Blazor UI:

1. Navigate to **Accounting → Journal Entries**
2. Click **Create**
3. Leave required fields empty
4. Click **Save**

You should now see specific error messages like:
- "ReferenceNumber: Reference number is required."
- "Description: Description is required."
- etc.

---

## Verifying the Fix

### Before Fix:
```
Bad Request Status: 400 Response:
```
(No content body returned)

### After Fix:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Validation Error",
  "status": 400,
  "detail": "One or more validation errors occurred",
  "instance": "/api/v1/accounting/journal-entries",
  "errors": {
    "ReferenceNumber": ["Reference number is required."],
    "Description": ["Description is required."]
  }
}
```

---

## Using Postman

1. **Set Method:** POST
2. **URL:** `https://localhost:7000/api/v1/accounting/journal-entries`
3. **Headers:**
   - Content-Type: `application/json`
   - Accept: `application/json`
4. **Body (raw JSON):**
   ```json
   {
     "date": "2025-11-03T00:00:00Z",
     "referenceNumber": "",
     "source": "",
     "description": ""
   }
   ```
5. **Send**

Expected: Full error response with `errors` object showing which fields failed validation.

---

## Response Headers to Check

The response should include:
```
Content-Type: application/problem+json
Status: 400 Bad Request
```

---

## Troubleshooting

### If still getting empty response:

1. **Restart the API server** to pick up the changes
   ```bash
   dotnet run --project /path/to/Server.csproj
   ```

2. **Check the logs** for any serialization errors

3. **Verify the build** succeeded:
   ```bash
   cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src
   dotnet build
   ```

4. **Clear browser cache** if testing from UI

5. **Check Content-Type header** in request is `application/json`

---

## What Was Fixed

The issue was that `ProblemDetails` properties weren't being set properly:

1. ✅ Added `problemDetails.Status = StatusCodes.Status400BadRequest`
2. ✅ Added `httpContext.Response.ContentType = "application/problem+json"`
3. ✅ Added `problemDetails.Title = "Validation Error"`

These properties are required for proper serialization of the ProblemDetails object according to RFC 7807.

---

**Status:** ✅ Fixed and Ready to Test

**Next Steps:**
1. Restart your API server
2. Run one of the test scenarios above
3. Verify you now see detailed validation errors

