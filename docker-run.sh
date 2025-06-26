#!/bin/bash

# Docker Build and Run Script for HTML to PDF Converter

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Print colored output
print_status() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

print_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Function to show usage
show_usage() {
    echo "Usage: $0 [build|run|stop|logs|clean|help]"
    echo ""
    echo "Commands:"
    echo "  build  - Build the Docker image"
    echo "  run    - Run the application using docker-compose"
    echo "  stop   - Stop the running containers"
    echo "  logs   - Show application logs"
    echo "  clean  - Remove containers and images"
    echo "  help   - Show this help message"
}

# Build Docker image
build_image() {
    print_status "Building Docker image..."
    docker-compose build
    print_success "Docker image built successfully!"
}

# Run the application
run_application() {
    print_status "Starting HTML to PDF Converter..."
    docker-compose up -d
    
    print_success "Application started!"
    print_status "Web Interface: http://localhost:8080"
    print_status "Swagger API: http://localhost:8080/swagger"
    print_status "Use 'docker-compose logs -f' to see logs"
}

# Stop the application
stop_application() {
    print_status "Stopping application..."
    docker-compose down
    print_success "Application stopped!"
}

# Show logs
show_logs() {
    print_status "Showing application logs..."
    docker-compose logs -f
}

# Clean up
clean_up() {
    print_warning "This will remove all containers and images. Are you sure? (y/N)"
    read -r response
    if [[ "$response" =~ ^([yY][eE][sS]|[yY])$ ]]; then
        print_status "Cleaning up..."
        docker-compose down --rmi all --volumes --remove-orphans
        print_success "Cleanup completed!"
    else
        print_status "Cleanup cancelled."
    fi
}

# Main script logic
case "${1:-help}" in
    build)
        build_image
        ;;
    run)
        run_application
        ;;
    stop)
        stop_application
        ;;
    logs)
        show_logs
        ;;
    clean)
        clean_up
        ;;
    help|*)
        show_usage
        ;;
esac
