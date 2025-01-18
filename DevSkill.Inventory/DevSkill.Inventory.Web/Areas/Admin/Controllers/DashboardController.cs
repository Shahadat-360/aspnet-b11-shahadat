using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            _logger.LogInformation("Dashboard page visited");
            return View();
        }
    }
}
