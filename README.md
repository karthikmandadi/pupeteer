# HTML to PDF Converter - MVP Application

A .NET Core Web API application that converts HTML content and web pages to PDF documents using PuppeteerSharp.

## Features

- 🔄 Convert HTML content to PDF
- 🌐 Convert web URLs to PDF
- ⚙️ Configurable PDF options (paper size, orientation, margins, etc.)
- 🎨 Beautiful web interface for testing
- 📋 Swagger API documentation
- 🧪 Test endpoint for quick verification

## API Endpoints

### 1. Convert HTML to PDF
- **POST** `/api/pdf/convert`
- **Body**: JSON object with HTML content and options
- **Response**: PDF file download

### 2. Convert URL to PDF
- **GET** `/api/pdf/convert-url?url={url}&format={format}&landscape={true/false}`
- **Parameters**: 
  - `url`: Website URL to convert
  - `format`: Paper format (A4, A3, Letter, Legal)
  - `landscape`: Orientation (true/false)
- **Response**: PDF file download

### 3. Test Conversion
- **GET** `/api/pdf/test`
- **Response**: Sample PDF file for testing

## Getting Started

### Prerequisites
- .NET 8.0 SDK (for local development)
- OR Docker Desktop (for containerized deployment)
- Windows/Linux/macOS

### Option 1: Run with Docker (Recommended)

#### Quick Start
```bash
# Windows (PowerShell)
.\docker-run.ps1 build
.\docker-run.ps1 run

# Linux/macOS (Bash)
chmod +x docker-run.sh
./docker-run.sh build
./docker-run.sh run

# Manual Docker commands
docker-compose build
docker-compose up -d
```

**Access the application:**
- Web Interface: http://localhost:8080
- Swagger API: http://localhost:8080/swagger

📋 See [DOCKER.md](DOCKER.md) for detailed Docker instructions.

### Option 2: Run Locally (.NET)

#### Installation & Running

1. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

2. **Build the application:**
   ```bash
   dotnet build
   ```

3. **Run the application:**
   ```bash
   dotnet run
   ```

4. **Access the application:**
   - Web Interface: `https://localhost:7205` or `http://localhost:5205`
   - Swagger API Docs: `https://localhost:7205/swagger`

## Usage Examples

### Using the Web Interface
1. Open `https://localhost:7205` in your browser
2. Choose between "HTML Content" or "Web URL" tabs
3. Configure your options (paper format, orientation, etc.)
4. Click "Convert to PDF" to download the result

### Using cURL

**Convert HTML to PDF:**
```bash
curl -X POST "https://localhost:7205/api/pdf/convert" \
     -H "Content-Type: application/json" \
     -d '{
       "html": "<html><body><h1>Hello World</h1></body></html>",
       "options": {
         "format": "A4",
         "landscape": false,
         "printBackground": true
       }
     }' \
     --output document.pdf
```

**Convert URL to PDF:**
```bash
curl "https://localhost:7205/api/pdf/convert-url?url=https://example.com&format=A4&landscape=false" \
     --output webpage.pdf
```

**Test Conversion:**
```bash
curl "https://localhost:7205/api/pdf/test" --output test.pdf
```

## Configuration Options

The application supports various PDF generation options:

- **Format**: A4, A3, A2, A1, A0, Legal, Letter, Tabloid, Ledger
- **Orientation**: Portrait or Landscape
- **Print Background**: Include/exclude background colors and images
- **Scale**: Zoom level (0.1 to 2.0)
- **Margins**: Custom margins for top, bottom, left, right
- **Headers/Footers**: Custom header and footer templates

## Project Structure

```
HtmlToPdfApp/
├── Controllers/
│   └── PdfController.cs       # API endpoints
├── Models/
│   └── PdfModels.cs          # Data transfer objects
├── Services/
│   ├── IPdfService.cs        # Service interface
│   └── PdfService.cs         # PDF conversion logic
├── Properties/
│   └── launchSettings.json   # Development settings
├── wwwroot/
│   └── index.html            # Web interface
├── Program.cs                # Application entry point
└── HtmlToPdfApp.csproj      # Project file
```

## Dependencies

- **PuppeteerSharp** (13.0.2): Headless Chrome automation for PDF generation
- **Microsoft.AspNetCore.OpenApi** (8.0.0): OpenAPI/Swagger support
- **Swashbuckle.AspNetCore** (6.4.0): Swagger UI

## Notes

- On first run, PuppeteerSharp will download Chromium browser (~170MB)
- The application runs headless Chrome instances for PDF generation
- Generated PDFs maintain the styling and layout of the original HTML/webpage
- CORS is enabled for cross-origin requests in development

## Troubleshooting

1. **Chromium download issues**: Ensure internet connection and sufficient disk space
2. **PDF generation errors**: Check HTML validity and CSS compatibility
3. **URL conversion failures**: Verify the target website is accessible and doesn't block automated requests

## Contributing

This is an MVP application. Feel free to extend it with additional features like:
- Authentication and authorization
- PDF template management
- Batch processing
- Database integration
- Advanced styling options
