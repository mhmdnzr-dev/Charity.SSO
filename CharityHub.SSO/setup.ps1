$ErrorActionPreference = "Stop"

$migrationsPath = "Data/Migrations"

function Write-Log {
    param (
        [string]$Message,
        [ConsoleColor]$Color = [ConsoleColor]::White
    )
    Write-Host $Message -ForegroundColor $Color
}

function Show-Progress {
    param ([string]$Message)
    Write-Host "$Message..." -NoNewline
    Start-Sleep -Seconds 1
    Write-Host " ✅ Done!" -ForegroundColor Green
}

function Run-DotnetCommand {
    param ([string]$Command)
    $output = Invoke-Expression $Command 2>&1
    $filteredOutput = $output | Where-Object { $_ -notmatch "Entity Framework tools version '.*' is older than that of the runtime" }
    if ($filteredOutput) { Write-Host $filteredOutput }
}

Write-Log "🚀 Starting Migration Process..." -Color Cyan

if (Test-Path $migrationsPath) {
    Write-Log "🗑  Removing old migrations..." -Color Yellow
    Remove-Item -Recurse -Force $migrationsPath
    Show-Progress "Migrations removed"
} else {
    Write-Log "✅ No existing migrations found. Skipping cleanup." -Color Gray
}

function Add-Migration {
    param ([string]$DbContext, [string]$MigrationName, [string]$Folder)
    Write-Log "📌 Creating migration for $DbContext..." -Color Cyan
    Run-DotnetCommand "dotnet ef migrations add $MigrationName -c $DbContext -o Data/Migrations/$Folder"
    Show-Progress "$DbContext migration added"
}

Add-Migration -DbContext "ApplicationDbContext" -MigrationName "Users2" -Folder "Application"
Add-Migration -DbContext "PersistedGrantDbContext" -MigrationName "PersistedGrantMigration" -Folder "PersistedGrants"
Add-Migration -DbContext "ConfigurationDbContext" -MigrationName "ConfigurationMigration" -Folder "Configuration"


function Update-Database {
    param ([string]$DbContext)
    Write-Log "📡 Updating database for $DbContext..." -Color Cyan
    Run-DotnetCommand "dotnet ef database update -c $DbContext"
    Show-Progress "$DbContext database updated"
}

Update-Database -DbContext "ApplicationDbContext"
Update-Database -DbContext "PersistedGrantDbContext"
Update-Database -DbContext "ConfigurationDbContext"

Write-Log "🛠  Building the project..." -Color Cyan
Run-DotnetCommand "dotnet build"
Show-Progress "Project build complete"

Write-Log "🌱 Seeding database..." -Color Cyan
Run-DotnetCommand "dotnet run /seed"
Show-Progress "Database seeding completed"

Write-Log "🎉 Migration process completed successfully!" -Color Green
