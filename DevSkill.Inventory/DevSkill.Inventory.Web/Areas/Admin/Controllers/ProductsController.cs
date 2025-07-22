using AutoMapper;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
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
    [Area("Admin"),Authorize(Policy=Permissions.ProductPage)]
    public class ProductsController(IMediator mediator, ILogger<ProductsController> logger,
        IMapper mapper,IImageService imageService) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<ProductsController> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IImageService _imageService = imageService;
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet,AllowAnonymous]
        public async Task<JsonResult> GetProductDetails(string ProductId,SalesType salesType)
        {
            var product = await _mediator.Send(new ProductGetByIdQuery { Id = ProductId });
            return Json(new
            {
                success = true,
                id = product.Id,
                code = product.Id,
                name = product.Name,
                stock = product.Stock,
                unitprice = salesType==SalesType.MRP? product.MRP : product.WholesalePrice,
                });
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


        [HttpGet,Authorize(Policy = Permissions.ProductUpdate)]
        public IActionResult StockUpdate()
        {
            var model = new ProductStockUpdateCommand();
            return PartialView("_ProductStockUpdateModalPartial", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.ProductUpdate)]
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
        [Authorize(Policy = Permissions.ProductUpdate)]
        public IActionResult DamageStockUpdate()
        {
            var model = new ProductDamageUpdateCommand();
            return PartialView("_ProductDamageUpdateModalPartial", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.ProductUpdate)]
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

        [HttpGet,Authorize(Policy = Permissions.ProductAdd)]
        public IActionResult Add()
        {
            var model = new ProductAddCommand();
            return PartialView("_ProductAddModalPartial", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.ProductAdd)]
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
        [Authorize(Policy = Permissions.ProductUpdate)]
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

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.ProductUpdate)]
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
                else
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product update Failed",
                        Type = ResponseType.Danger
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

        [HttpGet,AllowAnonymous]
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
        [Authorize(Policy = Permissions.ProductDelete)]
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
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx &&
                                      (sqlEx.Number == 547 || sqlEx.Number == 2292))
            {
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Cannot delete customer because it has related sales records",
                    Type = ResponseType.Danger
                });
                _logger.LogWarning(ex, "Failed to delete customer {CustomerId} due to foreign key constraint", Id);
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
                    data = data.Select(record =>
                    {
                        var imgUrl = string.IsNullOrEmpty(record.ImageUrl)? "":_imageService.GetPreSignedURL(record.ImageUrl);
                        return new[]
                        {
                            (++index).ToString(),
                            HttpUtility.HtmlEncode(imgUrl),
                            record.Id.ToString(),
                            HttpUtility.HtmlEncode(record.Name),
                            HttpUtility.HtmlEncode(record.CategoryName),
                            record.PurchasePrice.ToString("F2"),
                            record.MRP.ToString("F2"),
                            record.WholesalePrice.ToString("F2"),
                            record.Stock.ToString(),
                            record.LowStock.ToString(),
                            record.DamageStock.ToString()
                        };
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
