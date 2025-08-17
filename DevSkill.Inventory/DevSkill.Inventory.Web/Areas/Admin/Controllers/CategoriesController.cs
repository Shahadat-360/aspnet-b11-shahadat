using AutoMapper;
using DevSkill.Inventory.Application.Features.Categories.Commands;
using DevSkill.Inventory.Application.Features.Categories.Queries;
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
    [Area("Admin"),Authorize(Policy = Permissions.CategoryPage)]
    public class CategoriesController(IMediator mediator,IMapper mapper,
        ILogger<CategoriesController> logger) 
        : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CategoriesController> _logger = logger;
        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _mediator.Send(new CategoriesByQuery());
                return View(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching categories");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "An error occurred while fetching categories.",
                    Type = ResponseType.Danger,
                });
                return RedirectToAction("Index", "Settings");
            }
        }

        [HttpGet, Authorize(Policy = Permissions.CategoryAdd)]
        public IActionResult Add()
        {
            var model = new CategoryAddCommand();
            return PartialView("_CategoryAddModalPartial", model);
        }

        [HttpPost, Authorize(Policy = Permissions.CategoryAdd)]
        public async Task<IActionResult> AddAsync(CategoryAddCommand categoryAddCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(categoryAddCommand);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Category added successfully",
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
                    Message = "Error To Adding Category",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error To Adding Category");
            }
            return RedirectToAction("Index");
        }

        [HttpGet, Authorize(Policy = Permissions.CategoryUpdate)]
        public async Task<IActionResult> Update(Guid Id)
        {
            try
            {
                var category = await _mediator.Send(new CategoryGetByIdQuery { Id = Id });
                var model = _mapper.Map<CategoryUpdateCommand>(category);
                return PartialView("_CategoryUpdateModalPartial", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fetching Category Failed");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Category Fetching Failed",
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
        [Authorize(Policy = Permissions.CategoryUpdate)]
        public async Task<IActionResult> Update(CategoryUpdateCommand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(model);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Category updated successfully",
                        Type = ResponseType.Success
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Category update Failed",
                        Type = ResponseType.Danger
                    });
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Error updating Category",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error updating Category");
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.CategoryDelete)]
        public async Task<IActionResult> Delete(Guid Id)
        {
            try
            {
                await _mediator.Send(new CategoryDeleteCommand { Id = Id });
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Category deleted successfully",
                    Type = ResponseType.Success
                });
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx &&
                                      (sqlEx.Number == 547 || sqlEx.Number == 2292))
            {
                var message = "Cannot delete Category because it has used in Product";
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
                    Message = "Error Deleting Category",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error Deleting Category");
            }
            return RedirectToAction("Index");
        }


        [HttpGet,AllowAnonymous]
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

        [HttpPost, ValidateAntiForgeryToken,AllowAnonymous]
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
