namespace HtmlToPdfApp.Models
{
    public class HtmlToPdfRequest
    {
        public string Html { get; set; } = string.Empty;
        public string? Url { get; set; }
        public PdfOptions Options { get; set; } = new PdfOptions();
    }

    public class PdfOptions
    {
        public string Format { get; set; } = "A4"; // A4, A3, A2, A1, A0, Legal, Letter, Tabloid, Ledger
        public bool Landscape { get; set; } = false;
        public bool PrintBackground { get; set; } = true;
        public double Scale { get; set; } = 1.0;
        public string? MarginTop { get; set; } = "1cm";
        public string? MarginBottom { get; set; } = "1cm";
        public string? MarginLeft { get; set; } = "1cm";
        public string? MarginRight { get; set; } = "1cm";
        public string? HeaderTemplate { get; set; }
        public string? FooterTemplate { get; set; }
        public bool DisplayHeaderFooter { get; set; } = false;
    }

    public class PdfResponse
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public byte[]? PdfData { get; set; }
        public string? FileName { get; set; }
    }
}
