using DevSkill.Inventory.Application.Features.Units.Commands;
using DevSkill.Inventory.Application.Features.Units.Queries;
using DevSkill.Inventory.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UnitsController(IMediator mediator,ILogger<UnitsController> logger) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<UnitsController> _logger = logger;
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> SearchUnits(string term = "", int page = 1, int pageSize = 5)
        {
            try
            {
                var result = await _mediator.Send(new UnitSearchWithPaginationQuery { term = term, page = page, pageSize = pageSize });

                var results = result.Items.Select(c => new
                {
                    id = c.Id.ToString(),
                    text = c.Name,
                    status = c.Status.ToString(),
                    disabled = c.Status == Status.Inactive
                }).ToList();

                return Json(new
                {
                    results = results,
                    pagination = new
                    {
                        more = result.HasNextPage
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching categories");
                return Json(new { results = new List<object>(), pagination = new { more = false } });
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUnitSelect2([FromBody] UnitAddCommand unitAddCommand)
        {
            if (string.IsNullOrWhiteSpace(unitAddCommand.Name))
            {
                return BadRequest("Name cannot be empty");
            }
            var newCategory = await _mediator.Send(unitAddCommand);
            return Json(new
            {
                Id = newCategory.Id.ToString(),
                Name = newCategory.Name,
            });
        }
    }
}
