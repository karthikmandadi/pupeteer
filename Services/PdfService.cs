using PuppeteerSharp;
using PuppeteerSharp.Media;
using HtmlToPdfApp.Models;
using PuppeteerPdfOptions = PuppeteerSharp.PdfOptions;

namespace HtmlToPdfApp.Services
{
    public class PdfService : IPdfService
    {
        private readonly ILogger<PdfService> _logger;

        public PdfService(ILogger<PdfService> logger)
        {
            _logger = logger;
        }

        public async Task<PdfResponse> ConvertHtmlToPdfAsync(HtmlToPdfRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Html) && string.IsNullOrEmpty(request.Url))
                {
                    return new PdfResponse
                    {
                        Success = false,
                        ErrorMessage = "Either HTML content or URL must be provided"
                    };
                }

                // Download Chromium if not already downloaded (unless using system Chrome)
                var executablePath = Environment.GetEnvironmentVariable("PUPPETEER_EXECUTABLE_PATH");
                if (string.IsNullOrEmpty(executablePath))
                {
                    await new BrowserFetcher().DownloadAsync();
                }

                var launchOptions = new LaunchOptions
                {
                    Headless = true,
                    ExecutablePath = executablePath, // Use system Chrome if available
                    Args = new[] { 
                        "--no-sandbox", 
                        "--disable-setuid-sandbox", 
                        "--disable-dev-shm-usage",
                        "--disable-gpu",
                        "--no-first-run",
                        "--disable-extensions",
                        "--disable-background-timer-throttling",
                        "--disable-backgrounding-occluded-windows",
                        "--disable-renderer-backgrounding"
                    }
                };

                using var browser = await Puppeteer.LaunchAsync(launchOptions);
                using var page = await browser.NewPageAsync();

                // Set content or navigate to URL
                if (!string.IsNullOrEmpty(request.Html))
                {
                    await page.SetContentAsync(request.Html);
                }
                else if (!string.IsNullOrEmpty(request.Url))
                {
                    await page.GoToAsync(request.Url, new NavigationOptions
                    {
                        WaitUntil = new[] { WaitUntilNavigation.Networkidle0 }
                    });
                }

                // Configure PDF options
                var pdfOptions = new PuppeteerPdfOptions
                {
                    Format = GetPaperFormat(request.Options.Format),
                    Landscape = request.Options.Landscape,
                    PrintBackground = request.Options.PrintBackground,
                    Scale = (decimal)request.Options.Scale,
                    DisplayHeaderFooter = request.Options.DisplayHeaderFooter,
                    HeaderTemplate = request.Options.HeaderTemplate,
                    FooterTemplate = request.Options.FooterTemplate,
                    MarginOptions = new MarginOptions
                    {
                        Top = request.Options.MarginTop,
                        Bottom = request.Options.MarginBottom,
                        Left = request.Options.MarginLeft,
                        Right = request.Options.MarginRight
                    }
                };

                // Generate PDF
                var pdfBytes = await page.PdfDataAsync(pdfOptions);

                return new PdfResponse
                {
                    Success = true,
                    PdfData = pdfBytes,
                    FileName = $"document_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error converting HTML to PDF");
                return new PdfResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<PdfResponse> ConvertUrlToPdfAsync(string url, Models.PdfOptions? options = null)
        {
            var request = new HtmlToPdfRequest
            {
                Url = url,
                Options = options ?? new Models.PdfOptions()
            };

            return await ConvertHtmlToPdfAsync(request);
        }

        private static PaperFormat GetPaperFormat(string format)
        {
            return format.ToUpper() switch
            {
                "A4" => PaperFormat.A4,
                "A3" => PaperFormat.A3,
                "A2" => PaperFormat.A2,
                "A1" => PaperFormat.A1,
                "A0" => PaperFormat.A0,
                "LEGAL" => PaperFormat.Legal,
                "LETTER" => PaperFormat.Letter,
                "TABLOID" => PaperFormat.Tabloid,
                "LEDGER" => PaperFormat.Ledger,
                _ => PaperFormat.A4
            };
        }
    }
}
