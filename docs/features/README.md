# Feature docs

This folder groups all top-level feature Markdown documents for faster navigation.

Goal
- Move scattered .md feature documents (e.g., ACCOUNTING_*.md, DEFERRED_REVENUE_*.md, etc.) into this folder.
- Keep docs focused and easy to scan.

How the scripts work
- `scripts/move-md-to-docs.sh` can run in dry-run mode to show which files it would move.
- Run it with `--apply` to actually move files.

Notes
- The script only moves `.md` files from the repository root (top-level) into this folder.
- It will NOT descend into module folders. If you want to collect docs from subfolders too, use the `--recursive` flag.

If you'd like, I can run the script for you (dry-run first, then apply when you confirm).
