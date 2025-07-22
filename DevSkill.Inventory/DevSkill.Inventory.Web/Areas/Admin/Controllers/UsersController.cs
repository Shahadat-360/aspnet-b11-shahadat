using AutoMapper;
using DevSkill.Inventory.Application.Features.Users.Commands;
using DevSkill.Inventory.Application.Features.Users.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Policy = Permissions.UserPage)]
    public class UsersController(IMediator mediator, ILogger<UsersController> logger,
        IMapper mapper) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UsersController> _logger = logger;
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet, Authorize(Policy = Permissions.UserAdd)]
        public IActionResult Add()
        {
            var model = new UserAddCommand();
            return PartialView("_UserAddModalPartial", model);
        }

        [HttpPost, Authorize(Policy = Permissions.UserAdd)]
        public async Task<IActionResult> AddAsync(UserAddCommand userAddCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(userAddCommand);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "User added successfully",
                        Type = ResponseType.Success
                    });
                    return RedirectToAction("ViewUsers");
                }
                else
                {
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "There are validation errors. Please correct them and try again.",
                        Type = ResponseType.Danger
                    });
                    return RedirectToAction("ViewUsers");
                }
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Error To Adding User",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error To Adding User");
            }
            return RedirectToAction("ViewUsers");
        }

        [HttpGet, Authorize(Policy = Permissions.UserUpdate)]
        public async Task<IActionResult> Update(Guid Id)
        {
            try
            {
                var user = await _mediator.Send(new UserGetByIdQuery { Id = Id });
                var model = _mapper.Map<UserUpdateCommand>(user);
                return PartialView("_UserUpdateModalPartial", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fetching User Failed");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "User Fetching Failed",
                    Type = ResponseType.Danger
                });
                return Json(new
                {
                    success = false,
                    redirectUrl = Url.Action("ViewUsers")
                });
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.UserUpdate)]
        public async Task<IActionResult> Update(UserUpdateCommand userUpdateCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(userUpdateCommand);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "User updated successfully",
                        Type = ResponseType.Success
                    });
                    return RedirectToAction("ViewUsers");
                }
                else
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "User update Failed",
                        Type = ResponseType.Danger
                    });
                    return RedirectToAction("ViewUsers");
                }
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Error updating User",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error updating User");
                return RedirectToAction("ViewUsers");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.UserDelete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _mediator.Send(new UserDeleteCommand { Id = id });
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "User deleted successfully",
                    Type = ResponseType.Success
                });
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx &&
                                      (sqlEx.Number == 547 || sqlEx.Number == 2292))
            {
                var message = "Cannot delete user because it is referenced by other records.";
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = message,
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, message);
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Error deleting user.",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Unexpected error while deleting user.");
            }

            return RedirectToAction("ViewUsers");
        }

        public IActionResult ViewUsers()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetUsersJsonData([FromBody] UsersByQuery model)
        {
            try
            {
                var (data, total, totalDisplay) = await _mediator.Send(model);
                int index = 0;
                var users = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                                (++index).ToString(),
                                HttpUtility.HtmlEncode(record?.EmployeeName),
                                HttpUtility.HtmlEncode(record?.Company),
                                HttpUtility.HtmlEncode(record?.Email),
                                HttpUtility.HtmlEncode(record?.Mobile),
                                HttpUtility.HtmlEncode(record?.RoleName),
                                record?.Status.ToString(),
                                record?.Id.ToString()
                            })
                };
                return Json(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem in getting Users");
                return Json(UsersByQuery.EmptyResult);
            }
        }
    }
}