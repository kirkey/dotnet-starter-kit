#!/usr/bin/env bash
set -euo pipefail

REPO_ROOT="$(cd "$(dirname "$0")/.." && pwd)"
TARGET_DIR="$REPO_ROOT/docs/features"
DRY_RUN=true
RECURSIVE=false

usage(){
  cat <<EOF
Usage: $0 [--apply] [--recursive]

--apply       Apply the move. Without this flag, script runs in dry-run mode.
--recursive   Also move .md files from subdirectories (not just root).

EOF
}

if [ "$#" -gt 0 ]; then
  for arg in "$@"; do
    case "$arg" in
      --apply) DRY_RUN=false ;;
      --recursive) RECURSIVE=true ;;
      -h|--help) usage; exit 0 ;;
      *) echo "Unknown arg: $arg"; usage; exit 1 ;;
    esac
  done
fi

mkdir -p "$TARGET_DIR"

if [ "$RECURSIVE" = true ]; then
  FIND_DEPTH=""
else
  FIND_DEPTH="-maxdepth 1"
fi

mapfile -t FILES < <(cd "$REPO_ROOT" && find . $FIND_DEPTH -type f -name '*.md' -print | sed 's|^./||')

if [ ${#FILES[@]} -eq 0 ]; then
  echo "No markdown files found to move."
  exit 0
fi

printf "Found %d markdown files:\n" "${#FILES[@]}"
for f in "${FILES[@]}"; do
  printf "  %s\n" "$f"
done

echo
if [ "$DRY_RUN" = true ]; then
  echo "Dry run mode. No files moved. Run with --apply to move files."
  exit 0
fi

for f in "${FILES[@]}"; do
  dest="$TARGET_DIR/$f"
  destdir=$(dirname "$dest")
  mkdir -p "$destdir"
  mv "$REPO_ROOT/$f" "$dest"
  echo "Moved $f -> $dest"
done

echo "All files moved to $TARGET_DIR"
