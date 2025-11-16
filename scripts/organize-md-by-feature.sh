#!/usr/bin/env bash
set -euo pipefail

REPO_ROOT="$(cd "$(dirname "$0")/.." && pwd)"
TARGET_DIR="$REPO_ROOT/docs/features"
DRY_RUN=true
VERBOSE=false

usage(){
  cat <<EOF
Usage: $0 [--apply] [--verbose]

--apply    Actually move files. Without this flag, script runs in dry-run mode.
--verbose  Print each action as it's evaluated.

Behavior:
- Finds all .md/.MD/.markdown files recursively.
- Infers a "feature" prefix from filename by taking the leading token before '_' or '-'.
  Example: ACCOUNTING_FINAL_COMPLETE.md -> feature 'accounting'
- Moves files into docs/features/<feature>/ keeping original filename.

EOF
}

for arg in "$@"; do
  case "$arg" in
    --apply) DRY_RUN=false ;;
    --verbose) VERBOSE=true ;;
    -h|--help) usage; exit 0 ;;
    *) echo "Unknown arg: $arg"; usage; exit 1 ;;
  esac
done

mkdir -p "$TARGET_DIR"

# Find markdown files (case-insensitive)
mapfile -t FILES < <(cd "$REPO_ROOT" && find . -type f \( -iname '*.md' -o -iname '*.markdown' \) -print | sed 's|^./||')

if [ ${#FILES[@]} -eq 0 ]; then
  echo "No markdown files found."
  exit 0
fi

moves=()
for f in "${FILES[@]}"; do
  # Skip files already under docs/features to be safe
  case "$f" in
    docs/features/*) 
      $VERBOSE && echo "Skipping already organized: $f"
      continue;;
  esac

  # Extract filename
  fname=$(basename "$f")
  # Determine prefix: string before first '_' or '-' or space
  prefix=$(echo "$fname" | sed -E 's/^([A-Za-z0-9]+)[_\- ].*/\1/i')
  if [ -z "$prefix" ]; then
    prefix="misc"
  fi
  # normalize prefix to lowercase
  prefix=$(echo "$prefix" | tr '[:upper:]' '[:lower:]')

  destdir="$TARGET_DIR/$prefix"
  destpath="$destdir/$fname"
  moves+=("$f::$destpath")
done

if [ "${#moves[@]}" -eq 0 ]; then
  echo "No files to move."
  exit 0
fi

echo "Planned moves:"
for m in "${moves[@]}"; do
  src=${m%%::*}
  dst=${m##*::}
  echo "  $src -> $dst"
done

if [ "$DRY_RUN" = true ]; then
  echo "\nDry run mode: no files moved. Run with --apply to perform the moves."
  exit 0
fi

# Apply moves
for m in "${moves[@]}"; do
  src=${m%%::*}
  dst=${m##*::}
  dstdir=$(dirname "$dst")
  mkdir -p "$dstdir"
  mv "$REPO_ROOT/$src" "$dst"
  echo "Moved $src -> $dst"
done

echo "All markdown files moved into $TARGET_DIR"
