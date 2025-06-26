# Docker Configuration for HTML to PDF Converter

This document explains how to run the HTML to PDF Converter application using Docker containers.

## 🐳 Docker Setup

### Prerequisites
- Docker Desktop installed and running
- Docker Compose (included with Docker Desktop)

### Quick Start

#### Windows (PowerShell)
```powershell
# Build and run the application
.\docker-run.ps1 build
.\docker-run.ps1 run

# Access the application
# Web Interface: http://localhost:8080
# Swagger API: http://localhost:8080/swagger
```

#### Linux/macOS (Bash)
```bash
# Make script executable
chmod +x docker-run.sh

# Build and run the application
./docker-run.sh build
./docker-run.sh run

# Access the application
# Web Interface: http://localhost:8080
# Swagger API: http://localhost:8080/swagger
```

#### Manual Docker Commands
```bash
# Build the image
docker-compose build

# Run the application
docker-compose up -d

# View logs
docker-compose logs -f

# Stop the application
docker-compose down
```

## 📁 Docker Files Explanation

### Dockerfile
- **Multi-stage build** for optimized image size
- **Chrome installation** for Puppeteer PDF generation
- **Security hardening** with non-root user
- **Required dependencies** for headless Chrome

### docker-compose.yml
- **Port mapping**: 8080 (HTTP), 8081 (HTTPS)
- **Environment variables** for containerized Puppeteer
- **Health checks** to monitor application status
- **Security options** and increased shared memory for Chrome
- **Volume mounting** for logs (optional)

### .dockerignore
- Excludes unnecessary files from Docker build context
- Reduces image size and build time

## 🔧 Configuration

### Environment Variables
The following environment variables are configured in docker-compose.yml:

```yaml
environment:
  - ASPNETCORE_ENVIRONMENT=Development
  - ASPNETCORE_URLS=http://+:8080
  - PUPPETEER_SKIP_CHROMIUM_DOWNLOAD=true
  - PUPPETEER_EXECUTABLE_PATH=/usr/bin/google-chrome-stable
```

### Port Configuration
- **8080**: HTTP interface (Web UI and API)
- **8081**: HTTPS interface (optional)

To change ports, modify the `ports` section in docker-compose.yml:
```yaml
ports:
  - "3000:8080"  # Access on localhost:3000
  - "3001:8081"  # Access on localhost:3001
```

## 🚀 Usage Examples

### Using the Helper Scripts

#### PowerShell (Windows)
```powershell
# Build the Docker image
.\docker-run.ps1 build

# Start the application
.\docker-run.ps1 run

# View real-time logs
.\docker-run.ps1 logs

# Stop the application
.\docker-run.ps1 stop

# Clean up (remove containers and images)
.\docker-run.ps1 clean

# Show help
.\docker-run.ps1 help
```

#### Bash (Linux/macOS)
```bash
# Build the Docker image
./docker-run.sh build

# Start the application
./docker-run.sh run

# View real-time logs
./docker-run.sh logs

# Stop the application
./docker-run.sh stop

# Clean up (remove containers and images)
./docker-run.sh clean

# Show help
./docker-run.sh help
```

### Testing the Application

Once running, test the PDF conversion:

1. **Web Interface**: http://localhost:8080
2. **API Test**: http://localhost:8080/api/pdf/test
3. **Swagger Documentation**: http://localhost:8080/swagger

### API Usage Examples

```bash
# Test endpoint
curl "http://localhost:8080/api/pdf/test" --output test.pdf

# Convert URL to PDF
curl "http://localhost:8080/api/pdf/convert-url?url=https://example.com&format=A4" --output webpage.pdf

# Convert HTML to PDF
curl -X POST "http://localhost:8080/api/pdf/convert" \
     -H "Content-Type: application/json" \
     -d '{
       "html": "<html><body><h1>Hello Docker!</h1></body></html>",
       "options": {"format": "A4", "landscape": false}
     }' \
     --output document.pdf
```

## 🔍 Monitoring and Debugging

### View Application Logs
```bash
# Real-time logs
docker-compose logs -f

# Last 100 lines
docker-compose logs --tail=100

# Specific service logs
docker-compose logs htmltopdf-app
```

### Health Check
The application includes a health check that tests the `/api/pdf/test` endpoint every 30 seconds.

```bash
# Check container health
docker ps
# Look for "healthy" status in the STATUS column
```

### Debug Container Issues
```bash
# Access container shell
docker-compose exec htmltopdf-app /bin/bash

# Check Chrome installation
docker-compose exec htmltopdf-app google-chrome-stable --version

# View container resources
docker stats
```

## 🛠️ Troubleshooting

### Common Issues

1. **Docker not running**
   - Start Docker Desktop
   - Verify with: `docker version`

2. **Port already in use**
   - Change ports in docker-compose.yml
   - Or stop conflicting services

3. **Chrome/Puppeteer issues**
   - The container includes all necessary Chrome dependencies
   - Check logs for Puppeteer-specific errors

4. **Memory issues**
   - Increase Docker Desktop memory allocation
   - Default shared memory (2GB) should be sufficient

5. **Permission issues**
   - The application runs as non-root user for security
   - Logs volume may need permission adjustment

### Performance Optimization

1. **Increase memory** for better PDF generation performance
2. **Use SSD storage** for faster container startup
3. **Limit concurrent requests** to prevent resource exhaustion

## 🔒 Security Considerations

- Application runs as **non-root user**
- **No new privileges** security option enabled
- **Minimal attack surface** with only necessary packages
- **Chrome sandbox** enabled with required capabilities

## 📊 Resource Requirements

### Minimum Requirements
- **RAM**: 2GB (4GB recommended)
- **CPU**: 2 cores
- **Storage**: 2GB for image + data

### Recommended for Production
- **RAM**: 4GB+
- **CPU**: 4+ cores
- **Storage**: SSD preferred
- **Network**: Stable internet for URL conversions

## 🔄 Updates and Maintenance

### Updating the Application
```bash
# Pull latest changes
git pull

# Rebuild image
docker-compose build --no-cache

# Restart with new image
docker-compose down && docker-compose up -d
```

### Backup and Restore
```bash
# Export container (if needed)
docker save htmltopdf-converter > htmltopdf-backup.tar

# Import container
docker load < htmltopdf-backup.tar
```

This Docker setup provides a production-ready containerized environment for the HTML to PDF converter with proper security, monitoring, and ease of deployment.
