using AutoMapper;
using DevSkill.Inventory.Application.Features.UserRoles.Commands;
using DevSkill.Inventory.Application.Features.UserRoles.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Enums;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Policy = Permissions.UserRolePage)]
    public class UserRolesController(IMediator mediator, IMapper mapper,
        ILogger<UserRolesController>logger) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UserRolesController> _logger = logger;
        public async Task<IActionResult> Index()
        {
            try
            {
                var roles = await _mediator.Send(new UserRolesByQuery());
                return View(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user roles");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "An error occurred while fetching user roles.",
                    Type = ResponseType.Danger,
                });
                return RedirectToAction("Index", "Settings");
            }
        }

        [HttpGet, Authorize(Policy = Permissions.UserRoleAdd)]
        public IActionResult Add()
        {
            var model = new UserRoleAddCommand();
            return PartialView("_UserRoleAddModalPartial", model);
        }

        [HttpPost, Authorize(Policy = Permissions.UserRoleAdd)]
        public async Task<IActionResult> AddAsync(UserRoleAddCommand userRoleAddCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(userRoleAddCommand);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "User Role added successfully",
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
                    Message = "Error To Adding User Role",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error To Adding User Role");
            }
            return RedirectToAction("Index");
        }

        [HttpGet, Authorize(Policy = Permissions.UserRoleUpdate)]
        public async Task<IActionResult> Update(Guid Id)
        {
            try
            {
                var role = await _mediator.Send(new UserRoleGetByIdQuery { Id = Id });
                var model = _mapper.Map<UserRoleUpdateCommand>(role);
                return PartialView("_UserRoleUpdateModalPartial", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fetching User Role Failed");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "User Role Fetching Failed",
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
        [Authorize(Policy = Permissions.UserRoleUpdate)]
        public async Task<IActionResult> Update(UserRoleUpdateCommand userRoleUpdateCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(userRoleUpdateCommand);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "User Role updated successfully",
                        Type = ResponseType.Success
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "User Role update Failed",
                        Type = ResponseType.Danger
                    });
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Error updating User Role",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error updating User Role");
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.UserRoleDelete)]
        public async Task<IActionResult> Delete(Guid Id)
        {
            try
            {
                await _mediator.Send(new UserRoleDeleteCommand { Id = Id });
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "User Role deleted successfully",
                    Type = ResponseType.Success
                });
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx &&
                                      (sqlEx.Number == 547 || sqlEx.Number == 2292))
            {
                var message = "Cannot delete User Role Already Used";
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = message,
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, message);
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Error Deleting User Role",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error Deleting User Role");
            }
            return RedirectToAction("Index");
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> SearchUserRoles(string term = "", int page = 1, int pageSize = 5)
        {
            try
            {
                var result = await _mediator.Send(new UserRoleSearchWithPaginationQuery { term = term, page = page, pageSize = pageSize });
                var results = result.Items.Select(r => new
                {
                    id = r.Id.ToString(),
                    text = r.Name,
                    status = r.Status.ToString(),
                    disabled = r.Status == Status.Inactive
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
                _logger.LogError(ex, "Error searching user roles");
                return Json(new { results = new List<object>(), pagination = new { more = false } });
            }
        }
    }

}