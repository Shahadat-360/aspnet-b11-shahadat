using AutoMapper;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Enums;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Web;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController(IMediator mediator, ILogger<ProductsController> logger,
        IMapper mapper) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<ProductsController> _logger = logger;
        private readonly IMapper _mapper = mapper;
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ProductView(string Id)
        {
            try
            {
                var product = await _mediator.Send(new ProductGetByIdQuery { Id = Id });
                if (product == null)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product not found",
                        Type = ResponseType.Danger
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    var model = _mapper.Map<ProductViewDto>(product);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product view");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Error retrieving product view",
                    Type = ResponseType.Danger
                });
                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        public IActionResult StockUpdate()
        {
            var model = new ProductStockUpdateCommand();
            return PartialView("_ProductStockUpdateModalPartial", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> StockUpdate(ProductStockUpdateCommand productStockUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Invalid stock update request",
                        Type = ResponseType.Danger
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    await _mediator.Send(productStockUpdate);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Stock updated successfully",
                        Type = ResponseType.Success
                    });
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Error processing stock update",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error processing stock update");
                return RedirectToAction("Index");
            }

        }

        [HttpGet]
        public IActionResult DamageStockUpdate()
        {
            var model = new ProductDamageUpdateCommand();
            return PartialView("_ProductDamageUpdateModalPartial", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DamageStockUpdate(ProductDamageUpdateCommand productDamageUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Invalid damage stock update request",
                        Type = ResponseType.Danger
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    await _mediator.Send(productDamageUpdate);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Damage stock updated successfully",
                        Type = ResponseType.Success
                    });
                    return RedirectToAction("Index");
                }
            }
            catch(InvalidOperationException ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = ex.Message,
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Insufficient stock for damage update");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Error processing damage stock update",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error processing damage stock update");
                return RedirectToAction("Index");
            }
        }

        public IActionResult Add()
        {
            var model = new ProductAddCommand();
            return PartialView("_ProductAddModalPartial", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductAddCommand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(model);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product added successfully",
                        Type = ResponseType.Success
                    });
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Error adding product",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error adding product");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string Id)
        {
            try
            {
                var product = await _mediator.Send(new ProductGetByIdQuery { Id = Id });
                if (product == null)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product not found",
                        Type = ResponseType.Danger
                    });
                    return Json(new
                    {
                        success = false,
                        redirectUrl = Url.Action("Index")
                    });
                }
                else
                {
                    var model = _mapper.Map<ProductUpdateCommand>(product);
                    model.ImageBackup = model.ImageUrl;
                    return PartialView("_ProductUpdateModalPartial", model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product for update");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Error retrieving product for update",
                    Type = ResponseType.Danger
                });
                return Json(new
                {
                    success = false,
                    redirectUrl = Url.Action("Index")
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchProducts(string term = "", int page = 1, int pageSize = 5)
        {
            try
            {
                var result = await _mediator.Send(new ProductSearchWithPaginationQuery
                {
                    term = term,
                    page = page,
                    pageSize = pageSize
                });

                var results = result.Items.Select(p => new
                {
                    id = p.Id.ToString(),
                    text = p.Name
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
                _logger.LogError(ex, "Error searching products");
                return Json(new { results = new List<object>(), pagination = new { more = false } });
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductUpdateCommand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(model);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product updated successfully",
                        Type = ResponseType.Success
                    });
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Error updating product",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error updating product");
            }
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string Id)
        {
            try
            {
                await _mediator.Send(new ProductDeleteCommand { Id = Id });
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Product deleted successfully",
                    Type = ResponseType.Success
                });
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Error deleting product",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error deleting product");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> GetProductJsonData([FromBody] ProductsByQuery model)
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
                                HttpUtility.HtmlEncode(record.ImageUrl),
                                record.Id.ToString(),
                                HttpUtility.HtmlEncode(record.Name),
                                HttpUtility.HtmlEncode(record.CategoryName),
                                record.PurchasePrice.ToString("F2"),
                                record.MRP.ToString("F2"),
                                record.WholesalePrice.ToString("F2"),
                                record.Stock.ToString(),
                                record.LowStock.ToString(),
                                record.DamageStock.ToString()
                            })
                };
                return Json(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem in getting Products");
                return Json(ProductsByQuery.EmptyResult);
            }
        }

    }
}
