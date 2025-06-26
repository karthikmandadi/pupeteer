using Microsoft.AspNetCore.Mvc;
using HtmlToPdfApp.Models;
using HtmlToPdfApp.Services;

namespace HtmlToPdfApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfController : ControllerBase
    {
        private readonly IPdfService _pdfService;
        private readonly ILogger<PdfController> _logger;

        public PdfController(IPdfService pdfService, ILogger<PdfController> logger)
        {
            _pdfService = pdfService;
            _logger = logger;
        }

        /// <summary>
        /// Convert HTML content to PDF
        /// </summary>
        /// <param name="request">HTML to PDF conversion request</param>
        /// <returns>PDF file</returns>
        [HttpPost("convert")]
        public async Task<IActionResult> ConvertHtmlToPdf([FromBody] HtmlToPdfRequest request)
        {
            try
            {
                var result = await _pdfService.ConvertHtmlToPdfAsync(request);

                if (!result.Success)
                {
                    return BadRequest(new { error = result.ErrorMessage });
                }

                return File(result.PdfData!, "application/pdf", result.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ConvertHtmlToPdf endpoint");
                return StatusCode(500, new { error = "Internal server error occurred" });
            }
        }

        /// <summary>
        /// Convert URL to PDF
        /// </summary>
        /// <param name="url">URL to convert to PDF</param>
        /// <param name="format">Paper format (A4, A3, Letter, etc.)</param>
        /// <param name="landscape">Landscape orientation</param>
        /// <returns>PDF file</returns>
        [HttpGet("convert-url")]
        public async Task<IActionResult> ConvertUrlToPdf(
            [FromQuery] string url,
            [FromQuery] string format = "A4",
            [FromQuery] bool landscape = false)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    return BadRequest(new { error = "URL is required" });
                }

                var options = new Models.PdfOptions
                {
                    Format = format,
                    Landscape = landscape
                };

                var result = await _pdfService.ConvertUrlToPdfAsync(url, options);

                if (!result.Success)
                {
                    return BadRequest(new { error = result.ErrorMessage });
                }

                return File(result.PdfData!, "application/pdf", result.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ConvertUrlToPdf endpoint");
                return StatusCode(500, new { error = "Internal server error occurred" });
            }
        }

        /// <summary>
        /// Test endpoint to verify the API is working
        /// </summary>
        /// <returns>Test HTML converted to PDF</returns>
        [HttpGet("test")]
        public async Task<IActionResult> TestConversion()
        {
            try
            {
                var testHtml = @"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <title>Test PDF</title>
                        <style>
                            body { font-family: Arial, sans-serif; margin: 40px; }
                            h1 { color: #333; }
                            .highlight { background-color: #ffffcc; padding: 10px; }
                        </style>
                    </head>
                    <body>
                        <h1>HTML to PDF Conversion Test</h1>
                        <p>This is a test document to verify that HTML to PDF conversion is working correctly.</p>
                        <div class='highlight'>
                            <p><strong>Generated on:</strong> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"</p>
                        </div>
                        <ul>
                            <li>✓ HTML content rendering</li>
                            <li>✓ CSS styling</li>
                            <li>✓ PDF generation</li>
                        </ul>
                    </body>
                    </html>";

                var request = new HtmlToPdfRequest
                {
                    Html = testHtml,
                    Options = new Models.PdfOptions
                    {
                        Format = "A4",
                        PrintBackground = true
                    }
                };

                var result = await _pdfService.ConvertHtmlToPdfAsync(request);

                if (!result.Success)
                {
                    return BadRequest(new { error = result.ErrorMessage });
                }

                return File(result.PdfData!, "application/pdf", "test-document.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TestConversion endpoint");
                return StatusCode(500, new { error = "Internal server error occurred" });
            }
        }
    }
}
