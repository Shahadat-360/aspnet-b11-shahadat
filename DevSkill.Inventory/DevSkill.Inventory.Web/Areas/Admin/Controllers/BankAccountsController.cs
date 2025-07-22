using AutoMapper;
using DevSkill.Inventory.Application.Features.BankAccounts.Commands;
using DevSkill.Inventory.Application.Features.BankAccounts.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Enums;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Policy = Permissions.BankAccountPage)]
    public class BankAccountsController(IMediator mediator,IMapper mapper,
        ILogger<BankAccountsController> logger) 
        : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<BankAccountsController> _logger = logger;
        private readonly IMapper _mapper = mapper;
        public async Task<IActionResult> Index()
        {
            try
            {
                var bankAccounts = await _mediator.Send(new BankAccountsByQuery());
                return View(bankAccounts);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error fetching bank accounts");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "An error occurred while fetching bank accounts.",
                    Type = ResponseType.Danger,
                });
                return RedirectToAction("Index", "Dashboard");
            }
        }

        [HttpGet,Authorize(Policy = Permissions.BankAccountAdd)]
        public IActionResult Add()
        {
            var model = new BankAccountAddCommand();
            return PartialView("_BankAccountAddModalPartial", model);
        }

        [HttpPost,Authorize(Policy = Permissions.BankAccountAdd)]
        public async Task<IActionResult> AddAsync(BankAccountAddCommand bankAccountAddCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(bankAccountAddCommand);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Bank Account added successfully",
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
                    Message = "Error To Adding Bank Account",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error To Adding Bank Account");
            }
            return RedirectToAction("Index");
        }

        [HttpGet,Authorize(Policy = Permissions.BankAccountUpdate)]
        public async Task<IActionResult> Update(int Id)
        {
            try
            {
                var bankAccount =await _mediator.Send(new BankAccountGetByIdQuery { Id = Id });
                var model = _mapper.Map<BankAccountUpdateCommand>(bankAccount);
                return PartialView("_BankAccountUpdateModalPartial", model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Fetching Bank Account Failed");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Bank Account Fetching Failed",
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
        [Authorize(Policy = Permissions.BankAccountUpdate)]
        public async Task<IActionResult> Update(BankAccountUpdateCommand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(model);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Bank Account updated successfully",
                        Type = ResponseType.Success
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Bank Account update Failed",
                        Type = ResponseType.Danger
                    });
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Error updating Bank Account",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error updating Bank Account");
                return RedirectToAction("Index");
            }
        }

        [HttpPost,ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.BankAccountDelete)]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                await _mediator.Send(new BankAccountDeleteCommand { Id = Id });
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Bank Account deleted successfully",
                    Type = ResponseType.Success
                });
            }
            catch (InvalidOperationException ex)
            {
                var message = "Cannot delete Bank Account because it has used in sales";
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
                    Message = "Error Deleting Bank Account",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error Deleting Bank Account");
            }
            return RedirectToAction("Index");
        }

        [HttpGet,AllowAnonymous]
        public async Task<IActionResult> SearchAccounts(string term = "", int page = 1, int pageSize = 5)
        {
            try
            {
                var result = await _mediator.Send(new BankAccountSearchWithPaginationQuery { term = term, page = page, pageSize = pageSize });

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
                _logger.LogError(ex, "Error searching Bank Accounts");
                return Json(new { results = new List<object>(), pagination = new { more = false } });
            }
        }
    }
}
