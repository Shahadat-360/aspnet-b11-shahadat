using AutoMapper;
using DevSkill.Inventory.Application.Features.MobileAccounts.Commands;
using DevSkill.Inventory.Application.Features.MobileAccounts.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Enums;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Policy = Permissions.MobileAccountPage)]
    public class MobileAccountsController(IMediator mediator,IMapper mapper,
        ILogger<MobileAccountsController> logger) 
        : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<MobileAccountsController> _logger = logger;
        public async Task<IActionResult> Index()
        {
            try
            {
                var mobileAccounts = await _mediator.Send(new MobileAccountsByQuery());
                return View(mobileAccounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching mobile accounts");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "An error occurred while fetching mobile accounts.",
                    Type = ResponseType.Danger,
                });
                return RedirectToAction("Index", "Dashboard");
            }
        }

        [HttpGet, Authorize(Policy = Permissions.MobileAccountAdd)]
        public IActionResult Add()
        {
            var model = new MobileAccountAddCommand();
            return PartialView("_MobileAccountAddModalPartial", model);
        }

        [HttpPost, Authorize(Policy = Permissions.MobileAccountAdd)]
        public async Task<IActionResult> AddAsync(MobileAccountAddCommand mobileAccountAddCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(mobileAccountAddCommand);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Mobile Account added successfully",
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
                    Message = "Error To Adding Mobile Account",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error To Adding Mobile Account");
            }
            return RedirectToAction("Index");
        }

        [HttpGet, Authorize(Policy = Permissions.MobileAccountUpdate)]
        public async Task<IActionResult> Update(int Id)
        {
            try
            {
                var mobileAccount = await _mediator.Send(new MobileAccountGetByIdQuery { Id = Id });
                var model = _mapper.Map<MobileAccountUpdateCommand>(mobileAccount);
                return PartialView("_MobileAccountUpdateModalPartial", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fetching Mobile Account Failed");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Mobile Account Fetching Failed",
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
        [Authorize(Policy = Permissions.MobileAccountUpdate)]
        public async Task<IActionResult> Update(MobileAccountUpdateCommand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(model);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Mobile Account updated successfully",
                        Type = ResponseType.Success
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Mobile Account update Failed",
                        Type = ResponseType.Danger
                    });
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Error updating Mobile Account",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error updating Mobile Account");
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.MobileAccountDelete)]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                await _mediator.Send(new MobileAccountDeleteCommand { Id = Id });
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Mobile Account deleted successfully",
                    Type = ResponseType.Success
                });
            }
            catch (InvalidOperationException ex)
            {
                var message = "Cannot delete Mobile Account because it has used in sales";
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
                    Message = "Error Deleting Mobile Account",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error Deleting Mobile Account");
            }
            return RedirectToAction("Index");
        }

        [HttpGet,AllowAnonymous]
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
                _logger.LogError(ex, "Error searching Mobile Accounts");
                return Json(new { results = new List<object>(), pagination = new { more = false } });
            }
        }
    }
}
