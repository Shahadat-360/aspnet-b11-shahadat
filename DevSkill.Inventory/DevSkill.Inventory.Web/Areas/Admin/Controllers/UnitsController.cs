using AutoMapper;
using DevSkill.Inventory.Application.Features.Units.Commands;
using DevSkill.Inventory.Application.Features.Units.Queries;
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
    [Area("Admin"),Authorize(Policy = Permissions.UnitPage)]
    public class UnitsController(IMediator mediator,IMapper mapper,
        ILogger<UnitsController> logger) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UnitsController> _logger = logger;
        public async Task<IActionResult> Index()
        {
            try
            {
                var units = await _mediator.Send(new UnitsByQuery());
                return View(units);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching units");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "An error occurred while fetching units.",
                    Type = ResponseType.Danger,
                });
                return RedirectToAction("Index", "Settings");
            }
        }

        [HttpGet, Authorize(Policy = Permissions.UnitAdd)]
        public IActionResult Add()
        {
            var model = new UnitAddCommand();
            return PartialView("_UnitAddModalPartial", model);
        }

        [HttpPost, Authorize(Policy = Permissions.UnitAdd)]
        public async Task<IActionResult> AddAsync(UnitAddCommand unitAddCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(unitAddCommand);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Unit added successfully",
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
                    Message = "Error To Adding Unit",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error To Adding Unit");
            }
            return RedirectToAction("Index");
        }

        [HttpGet, Authorize(Policy = Permissions.UnitUpdate)]
        public async Task<IActionResult> Update(Guid Id)
        {
            try
            {
                var unit = await _mediator.Send(new UnitGetByIdQuery { Id = Id });
                var model = _mapper.Map<UnitUpdateCommand>(unit);
                return PartialView("_UnitUpdateModalPartial", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fetching Unit Failed");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Unit Fetching Failed",
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
        [Authorize(Policy = Permissions.UnitUpdate)]
        public async Task<IActionResult> Update(UnitUpdateCommand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(model);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Unit updated successfully",
                        Type = ResponseType.Success
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Unit update Failed",
                        Type = ResponseType.Danger
                    });
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Error updating Unit",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error updating Unit");
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.UnitDelete)]
        public async Task<IActionResult> Delete(Guid Id)
        {
            try
            {
                await _mediator.Send(new UnitDeleteCommand { Id = Id });
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Unit deleted successfully",
                    Type = ResponseType.Success
                });
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx &&
                                      (sqlEx.Number == 547 || sqlEx.Number == 2292))
            {
                var message = "Cannot delete Unit because it has used in Product";
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
                    Message = "Error Deleting Unit",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error Deleting Unit");
            }
            return RedirectToAction("Index");
        }

        [HttpGet, AllowAnonymous]
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
                _logger.LogError(ex, "Error searching units");
                return Json(new { results = new List<object>(), pagination = new { more = false } });
            }
        }

        [HttpPost, ValidateAntiForgeryToken,AllowAnonymous]
        public async Task<IActionResult> CreateUnitSelect2([FromBody] UnitAddCommand unitAddCommand)
        {
            if (string.IsNullOrWhiteSpace(unitAddCommand.Name))
            {
                return BadRequest("Name cannot be empty");
            }
            var newUnit = await _mediator.Send(unitAddCommand);
            return Json(new
            {
                Id = newUnit.Id.ToString(),
                Name = newUnit.Name,
            });
        }
    }
}
