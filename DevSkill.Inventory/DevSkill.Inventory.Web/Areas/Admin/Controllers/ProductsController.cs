using AutoMapper;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Add()
        {
            var model = new ProductAddCommand();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductAddCommand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(model);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Product added successfully",
                        Type = ResponseType.Success
                    });
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Error adding product",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error adding product");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid Id)
        {
            var model = new ProductUpdateCommand();
            try
            {
                var product = await _mediator.Send(new ProductGetByIdQuery { Id = Id });
                if (product == null)
                {
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Product not found",
                        Type = ResponseType.Danger
                    });
                    return RedirectToAction("Index");
                }
                model = _mapper.Map<ProductUpdateCommand>(product);
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Error fetching product",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error fetching product");
            }
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductUpdateCommand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(model);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Product updated successfully",
                        Type = ResponseType.Success
                    });
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Error updating product",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error updating product");
            }
            return View(model);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid Id)
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
                var products = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                                HttpUtility.HtmlEncode(record.Name),
                                HttpUtility.HtmlEncode(record.CategoryId),
                                record.Price.ToString(),
                                record.Id.ToString()
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
