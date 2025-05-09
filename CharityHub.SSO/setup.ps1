$ErrorActionPreference = "Stop"

$migrationsPath = "Data/Migrations"

#function Drop-Database {
#    param ([string]$DbContext)
#
#    Write-Log "💣 Dropping existing database for $DbContext..." -Color Red
#    Run-DotnetCommand "dotnet ef database drop -c $DbContext --force --yes"
#    Show-Progress "$DbContext dropped"
#}


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
    Write-Host " ✅ Done!" -ForegroundColor Green
}

function Run-DotnetCommand {
    param ([string]$Command)
    $output = Invoke-Expression $Command 2>&1 | Where-Object { $_ -notmatch "Entity Framework tools version '.*' is older than that of the runtime" }
    if ($output) { Write-Host $output }
}

function Manage-Migrations {
    Write-Log "🚀 Starting Migration Process..." -Color Cyan

    if (Test-Path $migrationsPath) {
        Write-Log "🗑  Removing old migrations..." -Color Yellow
        Remove-Item -Recurse -Force $migrationsPath
        Show-Progress "Migrations removed"
    } else {
        Write-Log "✅ No existing migrations found. Skipping cleanup." -Color Gray
    }
}

function Add-Migration {
    param (
        [string]$DbContext,
        [string]$MigrationName,
        [string]$Folder
    )
    Write-Log "📌 Creating migration for $DbContext..." -Color Cyan
    Run-DotnetCommand "dotnet ef migrations add $MigrationName -c $DbContext -o Data/Migrations/$Folder"
    Show-Progress "$DbContext migration added"
}

function Update-Database {
    param ([string]$DbContext)
    Write-Log "📡 Updating database for $DbContext..." -Color Cyan
    Run-DotnetCommand "dotnet ef database update -c $DbContext"
    Show-Progress "$DbContext database updated"
}

function Build-Project {
    Write-Log "🛠  Building the project..." -Color Cyan
    Run-DotnetCommand "dotnet build --no-incremental"
    Show-Progress "Project build complete"
}

function Seed-Database {
    Write-Log "🌱 Seeding database..." -Color Cyan
    Run-DotnetCommand "dotnet run /seed"
    Show-Progress "Database seeding completed"
}



# Execution
Manage-Migrations

#Drop-Database -DbContext "ApplicationDbContext"
#Drop-Database -DbContext "PersistedGrantDbContext"
#Drop-Database -DbContext "ConfigurationDbContext"

Add-Migration -DbContext "ApplicationDbContext" -MigrationName "Users2" -Folder "Application"
Add-Migration -DbContext "PersistedGrantDbContext" -MigrationName "PersistedGrantMigration" -Folder "PersistedGrants"
Add-Migration -DbContext "ConfigurationDbContext" -MigrationName "ConfigurationMigration" -Folder "Configuration"

Update-Database -DbContext "ApplicationDbContext"
Update-Database -DbContext "PersistedGrantDbContext"
Update-Database -DbContext "ConfigurationDbContext"

Build-Project
Seed-Database

Write-Log "🎉 Migration process completed successfully!" -Color Green
