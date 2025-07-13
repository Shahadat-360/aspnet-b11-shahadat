using DevSkill.Inventory.Application.Features.CashAccounts.Queries;
using DevSkill.Inventory.Application.Features.MobileAccounts.Queries;
using DevSkill.Inventory.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MobileAccountsController(IMediator mediator,ILogger<MobileAccountsController> logger) 
        : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<MobileAccountsController> _logger = logger;
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SearchAccounts(string term = "", int page = 1, int pageSize = 5)
        {
            try
            {
                var result = await _mediator.Send(new MobileAccountSearchWithPaginationQuery { term = term, page = page, pageSize = pageSize });

                var results = result.Items.Select(c => new
                {
                    id = c.Id.ToString(),
                    text = c.AccountName,
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
                _logger.LogError(ex, "Error searching Cash Accounts");
                return Json(new { results = new List<object>(), pagination = new { more = false } });
            }
        }
    }
}
