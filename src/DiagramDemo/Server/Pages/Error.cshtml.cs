using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DiagramDemo.Server.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Auto-generated code")]
        private readonly ILogger<ErrorModel> _logger;

        public string RequestId { get; set; }
        public bool ShowRequestId
            => !string.IsNullOrEmpty(RequestId);

        public ErrorModel(ILogger<ErrorModel> logger)
            => _logger = logger;

        public void OnGet()
            => RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}
