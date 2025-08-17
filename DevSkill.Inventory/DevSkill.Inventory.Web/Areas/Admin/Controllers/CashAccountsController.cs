using AutoMapper;
using DevSkill.Inventory.Application.Features.CashAccounts.Commands;
using DevSkill.Inventory.Application.Features.CashAccounts.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Enums;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Policy = Permissions.CashAccountPage)]
    public class CashAccountsController(IMediator mediator,IMapper mapper,
        ILogger<CashAccountsController> logger) 
        : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<CashAccountsController> _logger = logger;
        private readonly IMapper _mapper = mapper;
        public async Task<IActionResult> Index()
        {
            try
            {
                var cashAccounts = await _mediator.Send(new CashAccountsByQuery());
                return View(cashAccounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching cash accounts");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "An error occurred while fetching cash accounts.",
                    Type = ResponseType.Danger,
                });
                return RedirectToAction("Index", "Dashboard");
            }
        }

        [HttpGet, Authorize(Policy = Permissions.CashAccountAdd)]
        public IActionResult Add()
        {
            var model = new CashAccountAddCommand();
            return PartialView("_CashAccountAddModalPartial", model);
        }

        [HttpPost, Authorize(Policy = Permissions.CashAccountAdd)]
        public async Task<IActionResult> AddAsync(CashAccountAddCommand cashAccountAddCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(cashAccountAddCommand);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Cash Account added successfully",
                        Type = ResponseType.Success
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "There are validation errors. Please correct them and try again.",
                        Type = ResponseType.Danger
                    });
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Error To Adding Cash Account",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error To Adding Cash Account");
            }
            return RedirectToAction("Index");
        }

        [HttpGet, Authorize(Policy = Permissions.CashAccountUpdate)]
        public async Task<IActionResult> Update(int Id)
        {
            try
            {
                var cashAccount = await _mediator.Send(new CashAccountGetByIdQuery { Id = Id });
                var model = _mapper.Map<CashAccountUpdateCommand>(cashAccount);
                return PartialView("_CashAccountUpdateModalPartial", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fetching Cash Account Failed");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Cash Account Fetching Failed",
                    Type = ResponseType.Danger
                });
                return Json(new
                {
                    success = false,
                    redirectUrl = Url.Action("Index")
                });
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.CashAccountUpdate)]
        public async Task<IActionResult> Update(CashAccountUpdateCommand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(model);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Cash Account updated successfully",
                        Type = ResponseType.Success
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Cash Account update Failed",
                        Type = ResponseType.Danger
                    });
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Error updating Cash Account",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error updating Cash Account");
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.CashAccountDelete)]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                await _mediator.Send(new CashAccountDeleteCommand { Id = Id });
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Cash Account deleted successfully",
                    Type = ResponseType.Success
                });
            }
            catch (InvalidOperationException ex)
            {
                var message = "Cannot delete Cash Account because it has used in sales";
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = message,
                    Type = ResponseType.Danger
                });
                _logger.LogWarning(ex, message);
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Error Deleting Cash Account",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error Deleting Cash Account");
            }
            return RedirectToAction("Index");
        }

        [HttpGet,AllowAnonymous]
        public async Task<IActionResult> SearchAccounts(string term = "", int page = 1, int pageSize = 5)
        {
            try
            {
                var result = await _mediator.Send(new CashAccountSearchWithPaginationQuery { term = term, page = page, pageSize = pageSize });

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
