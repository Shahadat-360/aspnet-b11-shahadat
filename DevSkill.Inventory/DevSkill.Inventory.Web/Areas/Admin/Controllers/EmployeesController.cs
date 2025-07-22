using AutoMapper;
using DevSkill.Inventory.Application.Features.Employees.Commands;
using DevSkill.Inventory.Application.Features.Employees.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Enums;
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
    [Area("Admin"),Authorize(Policy = Permissions.EmployeePage)]
    public class EmployeesController(IMediator mediator,ILogger<EmployeesController> logger,
        IMapper mapper) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<EmployeesController> _logger = logger;
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet,Authorize(Policy = Permissions.EmployeeAdd)]
        public IActionResult Add()
        {
            var model = new EmployeeAddCommand();
            return PartialView("_EmployeeAddModalPartial",model);
        }

        [HttpPost, Authorize(Policy = Permissions.EmployeeAdd)]
        public async Task<IActionResult> AddAsync(EmployeeAddCommand employeeAddCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(employeeAddCommand);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Employee added successfully",
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
                    Message = "Error To Adding Employee",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error To Adding Employee");
            }
            return RedirectToAction("Index");
        }

        [HttpGet, Authorize(Policy = Permissions.EmployeeUpdate)]
        public async Task<IActionResult> Update(string Id)
        {
            try
            {
                var employee = await _mediator.Send(new EmployeeGetByIdQuery { Id = Id });
                var model = _mapper.Map<EmployeeUpdateCommand>(employee);
                return PartialView("_EmployeeUpdateModalPartial", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fetching Employee Failed");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Employee Fetching Failed",
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
        [Authorize(Policy = Permissions.EmployeeUpdate)]
        public async Task<IActionResult> Update(EmployeeUpdateCommand employeeUpdateCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(employeeUpdateCommand);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Employee updated successfully",
                        Type = ResponseType.Success
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Employee update Failed",
                        Type = ResponseType.Danger
                    });
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Error updating Employee",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error updating Employee");
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.EmployeeDelete)]
        public async Task<IActionResult> Delete(string Id)
        {
            try
            {
                await _mediator.Send(new EmployeeDeleteCommand { Id = Id });
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Employee deleted successfully",
                    Type = ResponseType.Success
                });
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx &&
                                      (sqlEx.Number == 547 || sqlEx.Number == 2292))
            {
                var message = "Cannot delete Employee because it is referenced by other records";
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
                    Message = "Error Deleting Employee",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error Deleting Employee");
            }
            return RedirectToAction("Index");
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> SearchEmployees(string term = "", int page = 1, int pageSize = 5)
        {
            try
            {
                var result = await _mediator.Send(new EmployeeSearchWithPaginationQuery { term = term, page = page, pageSize = pageSize });
                var results = result.Items.Select(e => new
                {
                    id = e.Id.ToString(),
                    text = e.Name,
                    status = e.Status.ToString(),
                    disabled = e.Status == Status.Inactive
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
                _logger.LogError(ex, "Error searching employees");
                return Json(new { results = new List<object>(), pagination = new { more = false } });
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetEmployeeJsonData([FromBody] EmployeesByQuery model)
        {
            try
            {
                var (data, total, totalDisplay) = await _mediator.Send(model);
                int index = 0;
                var products = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                                (++index).ToString(),
                                record.Id.ToString(),
                                HttpUtility.HtmlEncode(record.Name),
                                HttpUtility.HtmlEncode(record.Mobile),
                                HttpUtility.HtmlEncode(record.Email),
                                HttpUtility.HtmlEncode(record.Address),
                                record.JoiningDate.ToString("dd/MM/yyyy"),
                                record.Salary.ToString("F2"),
                                record.Status.ToString()
                            })
                };
                return Json(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem in getting Products");
                return Json(EmployeesByQuery.EmptyResult);
            }
        }
    }
}
