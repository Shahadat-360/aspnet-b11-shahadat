using AutoMapper;
using DevSkill.Inventory.Application.Features.Departments.Commands;
using DevSkill.Inventory.Application.Features.Departments.Queries;
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
    [Area("Admin"),Authorize(Policy = Permissions.DepartmentPage)]
    public class DepartmentsController(IMediator mediator,IMapper mapper,
        ILogger<DepartmentsController> logger) 
        : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<DepartmentsController> _logger = logger;
        public async Task<IActionResult> Index()
        {
            try
            {
                var departments = await _mediator.Send(new DepartmentsByQuery());
                return View(departments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching departments");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "An error occurred while fetching departments.",
                    Type = ResponseType.Danger,
                });
                return RedirectToAction("Index", "Settings");
            }
        }

        [HttpGet, Authorize(Policy = Permissions.DepartmentAdd)]
        public IActionResult Add()
        {
            var model = new DepartmentAddCommand();
            return PartialView("_DepartmentAddModalPartial", model);
        }

        [HttpPost, Authorize(Policy = Permissions.DepartmentAdd)]
        public async Task<IActionResult> AddAsync(DepartmentAddCommand departmentAddCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(departmentAddCommand);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Department added successfully",
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
                    Message = "Error To Adding Department",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error To Adding Department");
            }
            return RedirectToAction("Index");
        }

        [HttpGet, Authorize(Policy = Permissions.DepartmentUpdate)]
        public async Task<IActionResult> Update(int Id)
        {
            try
            {
                var department = await _mediator.Send(new DepartmentGetByIdQuery { Id = Id });
                var model = _mapper.Map<DepartmentUpdateCommand>(department);
                return PartialView("_DepartmentUpdateModalPartial", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fetching Department Failed");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Department Fetching Failed",
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
        [Authorize(Policy = Permissions.DepartmentUpdate)]
        public async Task<IActionResult> Update(DepartmentUpdateCommand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(model);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Department updated successfully",
                        Type = ResponseType.Success
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Department update Failed",
                        Type = ResponseType.Danger
                    });
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Error updating Department",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error updating Department");
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.DepartmentDelete)]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                await _mediator.Send(new DepartmentDeleteCommand { Id = Id });
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Department deleted successfully",
                    Type = ResponseType.Success
                });
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx &&
                                      (sqlEx.Number == 547 || sqlEx.Number == 2292))
            {
                var message = "Cannot delete Department because it's already used";
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
                    Message = "Error Deleting Department",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error Deleting Department");
            }
            return RedirectToAction("Index");
        }


        [HttpGet,AllowAnonymous]
        public async Task<IActionResult> SearchDepartments(string term = "", int page = 1, int pageSize = 5)
        {
            try
            {
                var result = await _mediator.Send(new DepartmentSearchWithPaginationQuery { term = term, page = page, pageSize = pageSize });

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
                _logger.LogError(ex, "Error searching departments");
                return Json(new { results = new List<object>(), pagination = new { more = false } });
            }
        }
    }
}
