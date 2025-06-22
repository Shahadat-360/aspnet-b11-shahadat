using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController(ILogger<DashboardController> logger) : Controller
    {
        private readonly ILogger<DashboardController> _logger = logger;

        public IActionResult Index()
        {
            _logger.LogInformation("Dashboard page visited");
            return View();
        }
    }
}
