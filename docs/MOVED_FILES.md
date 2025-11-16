# Moved top-level Markdown files into docs/

Summary
- I organized the repository's top-level Markdown files into a feature-oriented docs/ folder structure (examples: `docs/accounting`, `docs/fiscal-period-close`, `docs/hr`, `docs/payroll`, `docs/vendors`, `docs/payments`, `docs/misc`, etc.).
- Created `docs/README.md` (index) and a reusable script at `scripts/reorganize_docs_final.sh`.

What I changed
- Moved many top-level `.md` files into `docs/<feature>/` subfolders using pattern-based grouping.
- Files that looked ambiguous (UI patterns, templates, build notes) were placed into `docs/misc` by default.

How to verify locally (recommended)
1. From the repo root run:
   - `git status --short` (see moved files; if you used `git mv` you'll see them staged)
   - `git diff --name-only --staged` (confirm the list of renamed files)
2. To view the new docs tree:
   - `find docs -maxdepth 4 -type f | sort` or `ls -R docs`

If a file looks misplaced
- Move it to a different subfolder under `docs/` and commit. Example:
  - `git mv docs/misc/ONCLICK_PATTERN_STANDARDIZATION_COMPLETE.md docs/accounting/ui/`

Next steps I can take (pick any):
- Generate `docs/INDEX.md` that lists every file under `docs/` as a navigable index.
- Create `docs/README_ACCOUNTING.md` with a short TOC for the accounting docs.
- Re-run and produce an exact mapping CSV of original filenames -> new paths.

Notes
- I used `git mv` where a `.git` folder was present; otherwise `mv` was used. If you want a single commit for the reorg I recommend running:
  - `git add -A docs scripts && git commit -m "chore(docs): move top-level docs into feature folders"`

If you want, I can now produce a full file index (`docs/INDEX.md`) and commit the changes for you (if you want me to run the git commit here, confirm and I'll do it).
