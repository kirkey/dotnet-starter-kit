# Messaging Edit & Delete UI - Quick Reference

## How to Use

### Edit a Message
1. Find your message in the chat
2. Click the **⋮** (three dots) menu
3. Select **"Edit Message"**
4. Make your changes
5. Press **Ctrl+Enter** or click **"Save Changes"**

### Delete a Message
1. Find your message in the chat
2. Click the **⋮** (three dots) menu
3. Select **"Delete Message"** (shown in red)
4. Review the confirmation
5. Click **"Delete"**

## Keyboard Shortcuts

| Key | Action | Where |
|-----|--------|-------|
| `Ctrl` + `Enter` | Save changes | Edit Dialog |
| `Esc` | Cancel | Edit Dialog |
| `Enter` | Send message | Chat input |
| `Shift` + `Enter` | New line | Chat input |

## Edit Validation

Your edited message must:
- ✅ Not be empty
- ✅ Be under 5,000 characters
- ✅ Be different from the original

## Visual Indicators

| Indicator | Meaning |
|-----------|---------|
| **⋮** menu | Available actions (edit/delete) |
| **edited** badge | Message has been modified |
| **Updating message...** | Edit in progress |
| **Deleting message...** | Delete in progress |
| ✓ **Success!** | Operation completed |

## Features

- ✅ Character counter (shows remaining at < 100)
- ✅ Real-time validation
- ✅ Message preview in delete confirmation
- ✅ Smooth animations
- ✅ Loading indicators
- ✅ Error handling with details

## Notes

- Only your own messages show the ⋮ menu
- Deleted messages cannot be recovered
- Edited messages show an "edited" badge
- Changes save to database immediately
- Long messages auto-wrap at 70% width

---

*Need help? Check EDIT_DELETE_UI_IMPLEMENTATION.md for full details*

