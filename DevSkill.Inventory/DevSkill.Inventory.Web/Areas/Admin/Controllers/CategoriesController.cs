using DevSkill.Inventory.Application.Features.Categories.Commands;
using DevSkill.Inventory.Application.Features.Categories.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Enums;
using DevSkill.Inventory.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController(IMediator mediator,ILogger<CategoriesController> logger) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<CategoriesController> _logger = logger;
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SearchCategories(string term = "", int page = 1, int pageSize = 5)
        {
            try
            {
                var result = await _mediator.Send(new CategorySearchWithPaginationQuery { term = term, page = page, pageSize = pageSize });

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
        public async Task<IActionResult> CreateCategorySelect2([FromBody] CategoryAddCommand categoryAddCommand)
        {
            if (string.IsNullOrWhiteSpace(categoryAddCommand.Name))
            {
                return BadRequest("Name cannot be empty");
            }
            var newCategory = await _mediator.Send(categoryAddCommand);
            return Json(new
            {
                Id = newCategory.Id.ToString(),
                Name = newCategory.Name,
            });
        }
    }
}
