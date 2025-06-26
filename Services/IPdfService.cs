using HtmlToPdfApp.Models;

namespace HtmlToPdfApp.Services
{
    public interface IPdfService
    {
        Task<PdfResponse> ConvertHtmlToPdfAsync(HtmlToPdfRequest request);
        Task<PdfResponse> ConvertUrlToPdfAsync(string url, Models.PdfOptions? options = null);
    }
}
