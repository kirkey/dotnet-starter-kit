# Pick Lists UI - API Client Regeneration Required

## ⚠️ Action Required

The Pick Lists UI implementation is complete, but some workflow endpoints are not yet available in the generated API client.

## Missing Endpoints

The following endpoints need to be added to the API client (via NSwag regeneration):

1. **StartPickingEndpointAsync**
   - Method: `POST /store/picklists/{id}/start`
   - Command: `StartPickingCommand`
   - Changes status from "Assigned" to "InProgress"

2. **CompletePickingEndpointAsync**
   - Method: `POST /store/picklists/{id}/complete`
   - Command: `CompletePickingCommand`
   - Changes status from "InProgress" to "Completed"

3. **AssignPickListEndpointAsync**
   - Method: `POST /store/picklists/{id}/assign`
   - Command: `AssignPickListCommand`
   - Changes status from "Created" to "Assigned"

## Current Status

### ✅ Working Features
- Create pick lists
- Search pick lists with filters
- View pick list details
- Delete pick lists

### ⏳ Pending API Client (Temporarily Disabled)
- Assign pick list to picker
- Start picking workflow
- Complete picking workflow

These features show warning messages until the API client is regenerated.

## How to Fix

1. **Regenerate the API Client** using NSwag:
   ```bash
   cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src
   # Run NSwag code generation
   ```

2. **Uncomment the workflow calls** in `PickLists.razor.cs`:
   ```csharp
   // In StartPicking method (line ~167):
   await Client.StartPickingEndpointAsync("1", id, new StartPickingCommand()).ConfigureAwait(false);

   // In CompletePicking method (line ~193):
   await Client.CompletePickingEndpointAsync("1", id, new CompletePickingCommand()).ConfigureAwait(false);
   ```

3. **Update the success messages** from "Warning" to "Success"

4. **Uncomment the table reload** calls:
   ```csharp
   await _table.ReloadDataAsync();
   ```

## Backend Status

The backend endpoints are fully implemented according to:
- `/api/modules/Store/docs/Store_PickList_Implementation_Complete.md`

All workflow operations (Assign, Start, Complete) are ready on the backend side.

## Alternative Approach

If you need these features immediately, you can also manually add the endpoint methods to the IClient interface, but regenerating from the API specification is the recommended approach.

## Related Files

- **PickLists.razor.cs** - Contains the TODO comments for workflow methods
- **AssignPickListDialog.razor.cs** - Assignment dialog (ready to use)
- **PickListDetailsDialog.razor.cs** - Details viewer (working)

---

**Status**: UI Complete, Waiting for API Client Regeneration
**Date**: January 2025

