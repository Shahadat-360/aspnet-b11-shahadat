using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = Permissions.AccessSetup)]
    public class AccessSetupsController(IAccessSetupService accessSetupService) : Controller
    {
        private readonly IAccessSetupService _service = accessSetupService;

        public async Task<IActionResult> Index()
        {
            var roles = await _service.GetRolesAsync();
            return View(roles);
        }

        [HttpGet("Admin/AccessSetups/ManageClaims/{roleId}")]
        public async Task<IActionResult> ManageClaims(Guid roleId)
        {
            var claims = await _service.GetRoleClaimsAsync(roleId);
            return View(claims);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageClaims(string RoleId, List<string> SelectedClaims)
        {
            var success = await _service.UpdateRoleClaimsAsync(RoleId, SelectedClaims);
            if (!success) return NotFound();

            TempData.Put("ResponseMessage", new ResponseModel
            {
                Message = "Claims updated successfully",
                Type = ResponseType.Success
            });

            return RedirectToAction("Index");
        }

    }
}