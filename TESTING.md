# HTML to PDF Converter - Testing Guide

## Quick Start

1. **Build and Run the Application:**
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

2. **Access the Application:**
   - Web Interface: https://localhost:7205
   - API Documentation: https://localhost:7205/swagger

## Test Scenarios

### 1. Test via Web Interface
1. Open https://localhost:7205 in your browser
2. Use the pre-filled HTML content or enter your own
3. Adjust PDF options (format, orientation, etc.)
4. Click "Convert to PDF" to download

### 2. Test via API using PowerShell

**Test the basic conversion:**
```powershell
Invoke-RestMethod -Uri "https://localhost:7205/api/pdf/test" -OutFile "test.pdf"
```

**Convert HTML content:**
```powershell
$body = @{
    html = "<html><body><h1>Hello from PowerShell!</h1><p>This PDF was generated via API call.</p></body></html>"
    options = @{
        format = "A4"
        landscape = $false
        printBackground = $true
    }
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:7205/api/pdf/convert" -Method POST -Body $body -ContentType "application/json" -OutFile "powershell-test.pdf"
```

**Convert URL to PDF:**
```powershell
Invoke-RestMethod -Uri "https://localhost:7205/api/pdf/convert-url?url=https://example.com&format=A4&landscape=false" -OutFile "example-com.pdf"
```

### 3. Features to Test

✅ **HTML Content Conversion**
- Basic HTML rendering
- CSS styling support
- Custom fonts and colors
- Images and backgrounds

✅ **Web URL Conversion**
- Public website conversion
- JavaScript-rendered content
- Responsive layouts

✅ **PDF Options**
- Different paper formats (A4, A3, Letter, Legal)
- Portrait and landscape orientations
- Custom margins
- Background printing

✅ **Error Handling**
- Invalid HTML content
- Unreachable URLs
- Large document processing

## Sample HTML for Testing

```html
<!DOCTYPE html>
<html>
<head>
    <title>Advanced Test Document</title>
    <style>
        body { 
            font-family: 'Arial', sans-serif; 
            margin: 40px; 
            line-height: 1.6;
        }
        .header { 
            background: linear-gradient(45deg, #007bff, #28a745); 
            color: white; 
            padding: 20px; 
            border-radius: 10px;
            margin-bottom: 30px;
        }
        .content { 
            display: grid; 
            grid-template-columns: 1fr 1fr; 
            gap: 20px; 
        }
        .card { 
            border: 1px solid #ddd; 
            padding: 20px; 
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        table { 
            width: 100%; 
            border-collapse: collapse; 
            margin: 20px 0;
        }
        th, td { 
            border: 1px solid #ddd; 
            padding: 12px; 
            text-align: left; 
        }
        th { 
            background-color: #f8f9fa; 
        }
    </style>
</head>
<body>
    <div class="header">
        <h1>PDF Conversion Test Document</h1>
        <p>Testing advanced HTML features and CSS styling</p>
    </div>
    
    <div class="content">
        <div class="card">
            <h2>Features Tested</h2>
            <ul>
                <li>✓ CSS Grid Layout</li>
                <li>✓ Custom Fonts</li>
                <li>✓ Gradients</li>
                <li>✓ Box Shadows</li>
                <li>✓ Border Radius</li>
            </ul>
        </div>
        
        <div class="card">
            <h2>Data Table</h2>
            <table>
                <thead>
                    <tr>
                        <th>Feature</th>
                        <th>Status</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>HTML Rendering</td>
                        <td>✅ Working</td>
                    </tr>
                    <tr>
                        <td>CSS Styling</td>
                        <td>✅ Working</td>
                    </tr>
                    <tr>
                        <td>PDF Export</td>
                        <td>✅ Working</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    
    <footer style="margin-top: 40px; text-align: center; color: #666;">
        <p>Generated on: ${new Date().toLocaleString()}</p>
    </footer>
</body>
</html>
```

## Troubleshooting

**If the application doesn't start:**
1. Ensure .NET 8.0 SDK is installed
2. Check if ports 5205/7205 are available
3. Run `dotnet --version` to verify installation

**If PDF generation fails:**
1. Check internet connection (for Chromium download)
2. Ensure sufficient disk space
3. Verify HTML content is valid

**If URLs don't convert:**
1. Check if the URL is publicly accessible
2. Some sites block automated requests
3. Try with a simple test URL like https://example.com
