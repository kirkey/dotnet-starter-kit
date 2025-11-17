# CI/CD Integration Examples

This document provides examples of integrating the version notification system into various CI/CD pipelines.

## GitHub Actions

### Example 1: Automatic Version from Git Tag

```yaml
name: Deploy Blazor App

on:
  push:
    tags:
      - 'v*'

jobs:
  deploy:
    runs-on: ubuntu-latest
    
    steps:
      - name: Checkout code
        uses: actions/checkout@v3
      
      - name: Extract version from tag
        id: get_version
        run: echo "VERSION=${GITHUB_REF#refs/tags/v}" >> $GITHUB_OUTPUT
      
      - name: Update version.json
        working-directory: ./apps/blazor/client
        run: |
          chmod +x ./update-version.sh
          ./update-version.sh -v ${{ steps.get_version.outputs.VERSION }} -d "Release ${{ steps.get_version.outputs.VERSION }}"
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '9.0.x'
      
      - name: Restore dependencies
        run: dotnet restore
      
      - name: Build
        run: dotnet build --configuration Release --no-restore
      
      - name: Publish
        run: dotnet publish ./apps/blazor/client/Client.csproj -c Release -o ./publish
      
      - name: Deploy to Azure Static Web Apps
        uses: Azure/static-web-apps-deploy@v1
        with:
          azure_static_web_apps_api_token: ${{ secrets.AZURE_STATIC_WEB_APPS_API_TOKEN }}
          repo_token: ${{ secrets.GITHUB_TOKEN }}
          action: "upload"
          app_location: "./publish/wwwroot"
```

### Example 2: Automatic Version from Build Number

```yaml
name: Build and Deploy

on:
  push:
    branches: [ main ]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    
    steps:
      - uses: actions/checkout@v3
      
      - name: Generate version
        id: version
        run: |
          VERSION="1.0.${{ github.run_number }}"
          echo "VERSION=$VERSION" >> $GITHUB_OUTPUT
      
      - name: Update version.json
        working-directory: ./apps/blazor/client
        run: |
          chmod +x ./update-version.sh
          ./update-version.sh -v ${{ steps.version.outputs.VERSION }}
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '9.0.x'
      
      - name: Publish
        run: dotnet publish -c Release -o ./output
      
      - name: Upload artifact
        uses: actions/upload-artifact@v3
        with:
          name: blazor-app
          path: ./output/wwwroot
```

### Example 3: Manual Version with Workflow Dispatch

```yaml
name: Deploy with Custom Version

on:
  workflow_dispatch:
    inputs:
      version:
        description: 'Version number (e.g., 1.2.3)'
        required: true
        type: string
      description:
        description: 'Release description'
        required: false
        type: string
        default: 'Manual deployment'

jobs:
  deploy:
    runs-on: ubuntu-latest
    
    steps:
      - uses: actions/checkout@v3
      
      - name: Update version.json
        working-directory: ./apps/blazor/client
        run: |
          chmod +x ./update-version.sh
          ./update-version.sh -v ${{ inputs.version }} -d "${{ inputs.description }}"
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '9.0.x'
      
      - name: Publish
        run: dotnet publish -c Release
      
      # Add your deployment steps here
```

## Azure DevOps

### Example 1: Pipeline with Version from BuildId

```yaml
trigger:
  branches:
    include:
      - main

pool:
  vmImage: 'ubuntu-latest'

variables:
  buildConfiguration: 'Release'
  majorVersion: 1
  minorVersion: 0

steps:
  - task: UseDotNet@2
    displayName: 'Install .NET SDK'
    inputs:
      version: '9.0.x'
  
  - script: |
      VERSION="$(majorVersion).$(minorVersion).$(Build.BuildId)"
      echo "##vso[task.setvariable variable=appVersion]$VERSION"
      cd apps/blazor/client
      chmod +x ./update-version.sh
      ./update-version.sh -v $VERSION -d "Build $(Build.BuildNumber)"
    displayName: 'Update Version'
  
  - task: DotNetCoreCLI@2
    displayName: 'Restore'
    inputs:
      command: 'restore'
  
  - task: DotNetCoreCLI@2
    displayName: 'Build'
    inputs:
      command: 'build'
      arguments: '--configuration $(buildConfiguration) --no-restore'
  
  - task: DotNetCoreCLI@2
    displayName: 'Publish'
    inputs:
      command: 'publish'
      publishWebProjects: true
      arguments: '--configuration $(buildConfiguration) --output $(Build.ArtifactStagingDirectory)'
  
  - task: PublishBuildArtifacts@1
    displayName: 'Publish Artifacts'
    inputs:
      PathtoPublish: '$(Build.ArtifactStagingDirectory)'
      ArtifactName: 'drop'
```

### Example 2: Multi-Stage Pipeline with Version Control

```yaml
stages:
  - stage: Build
    jobs:
      - job: BuildApp
        pool:
          vmImage: 'ubuntu-latest'
        steps:
          - task: UseDotNet@2
            inputs:
              version: '9.0.x'
          
          - script: |
              VERSION="1.0.$(Build.BuildId)"
              cd apps/blazor/client
              chmod +x ./update-version.sh
              ./update-version.sh -v $VERSION
            displayName: 'Set Version'
          
          - task: DotNetCoreCLI@2
            inputs:
              command: 'publish'
              arguments: '-c Release -o $(Build.ArtifactStagingDirectory)'
          
          - publish: $(Build.ArtifactStagingDirectory)
            artifact: blazor-app

  - stage: Deploy
    dependsOn: Build
    jobs:
      - deployment: DeployToProduction
        environment: 'production'
        strategy:
          runOnce:
            deploy:
              steps:
                - download: current
                  artifact: blazor-app
                # Add deployment steps
```

## GitLab CI

### Example: GitLab CI with Semantic Versioning

```yaml
stages:
  - version
  - build
  - deploy

variables:
  MAJOR_VERSION: "1"
  MINOR_VERSION: "0"

update_version:
  stage: version
  image: mcr.microsoft.com/dotnet/sdk:9.0
  script:
    - VERSION="$MAJOR_VERSION.$MINOR_VERSION.$CI_PIPELINE_ID"
    - cd apps/blazor/client
    - chmod +x ./update-version.sh
    - ./update-version.sh -v $VERSION -d "Pipeline $CI_PIPELINE_ID"
  artifacts:
    paths:
      - apps/blazor/client/wwwroot/version.json
    expire_in: 1 hour

build:
  stage: build
  image: mcr.microsoft.com/dotnet/sdk:9.0
  dependencies:
    - update_version
  script:
    - dotnet restore
    - dotnet build -c Release
    - dotnet publish -c Release -o ./publish
  artifacts:
    paths:
      - ./publish
    expire_in: 1 day

deploy:
  stage: deploy
  image: alpine:latest
  dependencies:
    - build
  script:
    # Add your deployment commands here
    - echo "Deploying version $VERSION"
  only:
    - main
```

## Jenkins

### Example: Jenkinsfile with Version Management

```groovy
pipeline {
    agent any
    
    environment {
        MAJOR_VERSION = '1'
        MINOR_VERSION = '0'
        APP_VERSION = "${MAJOR_VERSION}.${MINOR_VERSION}.${BUILD_NUMBER}"
    }
    
    stages {
        stage('Update Version') {
            steps {
                script {
                    dir('apps/blazor/client') {
                        sh '''
                            chmod +x ./update-version.sh
                            ./update-version.sh -v ${APP_VERSION} -d "Jenkins Build ${BUILD_NUMBER}"
                        '''
                    }
                }
            }
        }
        
        stage('Restore') {
            steps {
                sh 'dotnet restore'
            }
        }
        
        stage('Build') {
            steps {
                sh 'dotnet build -c Release --no-restore'
            }
        }
        
        stage('Publish') {
            steps {
                sh 'dotnet publish -c Release -o ./publish'
            }
        }
        
        stage('Deploy') {
            steps {
                // Add your deployment steps
                echo "Deploying version ${APP_VERSION}"
            }
        }
    }
    
    post {
        success {
            archiveArtifacts artifacts: 'publish/**/*', fingerprint: true
        }
    }
}
```

## Docker Build

### Dockerfile with Version Update

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["FSH.Starter.sln", "./"]
COPY ["apps/blazor/client/Client.csproj", "apps/blazor/client/"]
COPY ["apps/blazor/infrastructure/Infrastructure.csproj", "apps/blazor/infrastructure/"]
COPY ["Shared/Shared.csproj", "Shared/"]

# Restore dependencies
RUN dotnet restore "apps/blazor/client/Client.csproj"

# Copy all source code
COPY . .

# Update version
ARG VERSION=1.0.0
WORKDIR "/src/apps/blazor/client"
RUN chmod +x ./update-version.sh && \
    ./update-version.sh -v ${VERSION} -d "Docker build"

# Build and publish
RUN dotnet publish "Client.csproj" -c Release -o /app/publish

# Runtime image
FROM nginx:alpine
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
COPY apps/blazor/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
```

### Docker Compose with Build Args

```yaml
version: '3.8'

services:
  blazor-client:
    build:
      context: .
      dockerfile: apps/blazor/Dockerfile.Blazor
      args:
        - VERSION=1.0.${BUILD_NUMBER:-0}
    ports:
      - "8080:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
```

## Automated Version from Git

### Script: get-version-from-git.sh

```bash
#!/bin/bash
# Gets version from git tags and commits

# Get latest tag
LATEST_TAG=$(git describe --tags --abbrev=0 2>/dev/null || echo "v0.0.0")
VERSION=${LATEST_TAG#v}

# Get commits since last tag
COMMITS_SINCE_TAG=$(git rev-list $LATEST_TAG..HEAD --count 2>/dev/null || echo "0")

if [ "$COMMITS_SINCE_TAG" -gt 0 ]; then
    # Add commit count to version
    VERSION="${VERSION}.${COMMITS_SINCE_TAG}"
fi

# Get short commit hash
COMMIT_HASH=$(git rev-parse --short HEAD)

echo "Version: $VERSION (commit: $COMMIT_HASH)"

# Update version.json
cd apps/blazor/client
./update-version.sh -v "$VERSION" -d "Git commit $COMMIT_HASH"
```

### Usage in CI:

```yaml
- name: Get version from git
  run: |
    chmod +x ./get-version-from-git.sh
    ./get-version-from-git.sh
```

## Version from Package.json (Alternative)

If you also maintain a package.json:

```bash
#!/bin/bash
# Sync version from package.json

if [ -f "package.json" ]; then
    VERSION=$(node -p "require('./package.json').version")
    cd apps/blazor/client
    ./update-version.sh -v "$VERSION"
else
    echo "package.json not found"
    exit 1
fi
```

## Best Practices

1. **Consistent Versioning**: Use the same version scheme across all deployment methods
2. **Automated Updates**: Always update version.json automatically in your pipeline
3. **Version Validation**: Validate version format before deployment
4. **Rollback Support**: Keep version history for potential rollbacks
5. **Environment Variables**: Use environment variables for version components
6. **Artifact Tagging**: Tag build artifacts with version numbers
7. **Changelog Generation**: Automatically generate changelogs from commits

## Testing Version Updates in CI

Add a test stage to verify version.json:

```yaml
- name: Verify Version
  run: |
    VERSION_FILE="apps/blazor/client/wwwroot/version.json"
    if [ ! -f "$VERSION_FILE" ]; then
        echo "ERROR: version.json not found"
        exit 1
    fi
    
    VERSION=$(jq -r '.version' $VERSION_FILE)
    if [ -z "$VERSION" ] || [ "$VERSION" = "null" ]; then
        echo "ERROR: Invalid version in version.json"
        exit 1
    fi
    
    echo "✓ Version validated: $VERSION"
```

## Notification on Deployment

Send notifications when new versions are deployed:

```yaml
- name: Notify Deployment
  if: success()
  run: |
    VERSION=$(jq -r '.version' apps/blazor/client/wwwroot/version.json)
    curl -X POST $SLACK_WEBHOOK_URL \
      -H 'Content-Type: application/json' \
      -d "{\"text\":\"🚀 Blazor App v$VERSION deployed successfully!\"}"
```

---

Choose the example that best fits your CI/CD platform and customize it for your needs!

