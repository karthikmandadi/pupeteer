# PowerShell Script for Docker Operations - HTML to PDF Converter

param(
    [Parameter(Position=0)]
    [ValidateSet("build", "run", "stop", "logs", "clean", "help", "")]
    [string]$Command = "help"
)

# Colors for output
function Write-StatusInfo {
    param([string]$Message)
    Write-Host "[INFO] $Message" -ForegroundColor Blue
}

function Write-StatusSuccess {
    param([string]$Message)
    Write-Host "[SUCCESS] $Message" -ForegroundColor Green
}

function Write-StatusWarning {
    param([string]$Message)
    Write-Host "[WARNING] $Message" -ForegroundColor Yellow
}

function Write-StatusError {
    param([string]$Message)
    Write-Host "[ERROR] $Message" -ForegroundColor Red
}

# Function to show usage
function Show-Usage {
    Write-Host "Usage: .\docker-run.ps1 [build|run|stop|logs|clean|help]" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Commands:" -ForegroundColor Cyan
    Write-Host "  build  - Build the Docker image"
    Write-Host "  run    - Run the application using docker-compose"
    Write-Host "  stop   - Stop the running containers"
    Write-Host "  logs   - Show application logs"
    Write-Host "  clean  - Remove containers and images"
    Write-Host "  help   - Show this help message"
    Write-Host ""
    Write-Host "Examples:" -ForegroundColor Cyan
    Write-Host "  .\docker-run.ps1 build"
    Write-Host "  .\docker-run.ps1 run"
    Write-Host "  .\docker-run.ps1 stop"
}

# Build Docker image
function Build-Image {
    Write-StatusInfo "Building Docker image..."
    try {
        docker-compose build
        Write-StatusSuccess "Docker image built successfully!"
    }
    catch {
        Write-StatusError "Failed to build Docker image: $_"
        exit 1
    }
}

# Run the application
function Start-Application {
    Write-StatusInfo "Starting HTML to PDF Converter..."
    try {
        docker-compose up -d
        Write-StatusSuccess "Application started!"
        Write-StatusInfo "Web Interface: http://localhost:8080"
        Write-StatusInfo "Swagger API: http://localhost:8080/swagger"
        Write-StatusInfo "Use 'docker-compose logs -f' to see logs"
    }
    catch {
        Write-StatusError "Failed to start application: $_"
        exit 1
    }
}

# Stop the application
function Stop-Application {
    Write-StatusInfo "Stopping application..."
    try {
        docker-compose down
        Write-StatusSuccess "Application stopped!"
    }
    catch {
        Write-StatusError "Failed to stop application: $_"
        exit 1
    }
}

# Show logs
function Show-Logs {
    Write-StatusInfo "Showing application logs..."
    try {
        docker-compose logs -f
    }
    catch {
        Write-StatusError "Failed to show logs: $_"
        exit 1
    }
}

# Clean up
function Clean-Up {
    Write-StatusWarning "This will remove all containers and images. Are you sure? (y/N)"
    $response = Read-Host
    if ($response -match '^[yY]([eE][sS])?$') {
        Write-StatusInfo "Cleaning up..."
        try {
            docker-compose down --rmi all --volumes --remove-orphans
            Write-StatusSuccess "Cleanup completed!"
        }
        catch {
            Write-StatusError "Failed to clean up: $_"
            exit 1
        }
    }
    else {
        Write-StatusInfo "Cleanup cancelled."
    }
}

# Check if Docker is running
function Test-DockerRunning {
    try {
        docker version | Out-Null
        return $true
    }
    catch {
        Write-StatusError "Docker is not running or not installed. Please start Docker Desktop."
        return $false
    }
}

# Main script logic
if (-not (Test-DockerRunning)) {
    exit 1
}

switch ($Command.ToLower()) {
    "build" {
        Build-Image
    }
    "run" {
        Start-Application
    }
    "stop" {
        Stop-Application
    }
    "logs" {
        Show-Logs
    }
    "clean" {
        Clean-Up
    }
    default {
        Show-Usage
    }
}
