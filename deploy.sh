#!/usr/bin/env bash

# Fail on error
set -e

# Verify we are in the project root directory
if [ ! -f "DashboardEstadisticaII.csproj" ]; then
    echo "Error: Please run this script from the project root directory (where DashboardEstadisticaII.csproj is located)."
    exit 1
fi

# Get remote URL and repository name
REMOTE_URL=$(git remote get-url origin 2>/dev/null || true)
if [ -z "$REMOTE_URL" ]; then
    echo "Error: No git remote 'origin' found. Please run 'git remote add origin <url>' first."
    exit 1
fi

REPO_NAME=$(basename -s .git "$REMOTE_URL")
echo "Deploying Blazor WASM project to GitHub Pages..."
echo "Repository name: $REPO_NAME"
echo "Remote URL: $REMOTE_URL"

# Clear any previous publish output
PUBLISH_DIR="release/wwwroot"
rm -rf release

# Publish Blazor WASM project
echo "Building and publishing the app in Release mode..."
dotnet publish -c Release -o release

INDEX_FILE="$PUBLISH_DIR/index.html"
if [ ! -f "$INDEX_FILE" ]; then
    echo "Error: Published index.html not found at $INDEX_FILE"
    exit 1
fi

# Add .nojekyll file to bypass Jekyll processing on GitHub Pages (required for _framework folder)
touch "$PUBLISH_DIR/.nojekyll"

# Adjust <base href="/" /> in index.html to match GitHub Pages path
# Using sed to replace <base href="..." /> with <base href="/REPO_NAME/" />
echo "Updating base href to /$REPO_NAME/..."
if [[ "$OSTYPE" == "darwin"* ]]; then
    # macOS sed requires an empty string argument for -i
    sed -i '' "s|<base href=\"/\" />|<base href=\"/$REPO_NAME/\" />|g" "$INDEX_FILE"
else
    # Linux sed
    sed -i "s|<base href=\"/\" />|<base href=\"/$REPO_NAME/\" />|g" "$INDEX_FILE"
fi

# Create a 404.html (copy of index.html) to support SPA client-side routing on direct navigation / page reloads
cp "$INDEX_FILE" "$PUBLISH_DIR/404.html"

echo "Committing and pushing to gh-pages branch..."

# Go to the published output directory
cd "$PUBLISH_DIR"

# Initialize temporary git repo in the output directory
git init
git checkout -B gh-pages

git config user.name "${GIT_COMMITTER_NAME:-github-actions[bot]}"
git config user.email "${GIT_COMMITTER_EMAIL:-github-actions[bot]@users.noreply.github.com}"
git remote remove origin 2>/dev/null || true
git remote add origin "$REMOTE_URL"

git add .
git commit -m "Deploy to GitHub Pages $(date -u +%Y-%m-%dT%H:%M:%SZ)" --allow-empty

# Force push to the gh-pages branch of the remote repository
git push -f origin gh-pages

echo "Successfully deployed to GitHub Pages!"
