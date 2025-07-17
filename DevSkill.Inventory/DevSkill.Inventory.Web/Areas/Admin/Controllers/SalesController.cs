using AutoMapper;
using DevSkill.Inventory.Application.Features.Sales.Commands;
using DevSkill.Inventory.Application.Features.Sales.Queries;
using DevSkill.Inventory.Domain.Enums;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SalesController(IMediator mediator,ILogger<SalesController> logger,IMapper mapper) 
        : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<SalesController> _logger = logger;
        private readonly IMapper _mapper = mapper;
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            var model = new SaleAddCommand();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(SaleAddCommand saleAddCommand)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Please FullFil Essential Field",
                        Type = ResponseType.Danger,
                    });
                    return View(saleAddCommand);
                }
                else
                {
                    await _mediator.Send(saleAddCommand);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Sale Added Successfully",
                        Type = ResponseType.Success
                    });
                    return RedirectToAction("Index");
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding sale");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "An error occurred while adding the sale. Please try again.",
                    Type = ResponseType.Danger
                });
                return View(saleAddCommand); 
            }
        }

        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogError($"Sale Id Is Not Found");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Invalid Sale ID",
                    Type = ResponseType.Danger
                });
                return RedirectToAction("Index");
            }
            else
            {
                var model = await _mediator.Send(new SaleGetByIdQuery { Id = id });
                var saleUpdateModel = _mapper.Map<SaleUpdateCommand>(model);
                return View(saleUpdateModel);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(SaleUpdateCommand saleUpdateCommand)
        {
            try
            {
                await _mediator.Send(saleUpdateCommand);
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Sale Updated Successfully",
                    Type = ResponseType.Success
                });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Sale Update Failed");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Sale Update Failed",
                    Type = ResponseType.Danger
                });
                return RedirectToAction("Index");
            }
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                if (id == null)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Sale id not found",
                        Type = ResponseType.Danger
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    await _mediator.Send(new SaleDeleteCommand { id = id });
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Sale Deleted Successfully",
                        Type = ResponseType.Success
                    });
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sale Update Failed");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Sale Update Failed",
                    Type = ResponseType.Danger
                });
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> SaleView(string id)
        {
            try
            {
                if (id == null)
                {
                    var message = "Invalid Sale ID";
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = message,
                        Type = ResponseType.Danger
                    });
                    _logger.LogError(message);
                    return RedirectToAction("Index");
                }
                else
                {
                    var SaleView = await _mediator.Send(new SaleGetByIdQuery { Id = id });
                    return View(SaleView);
                }
            }
            catch (Exception ex)
            {
                {
                    var message = "Sale can't able to view";
                    _logger.LogError(ex, message);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = message,
                        Type = ResponseType.Danger
                    });
                }
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Payment(string Id)
        {
            var sale = await _mediator.Send(new SaleGetByIdQuery { Id = Id });
            if (sale == null)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Sale not found",
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
                var model = _mapper.Map<SalePaymentUpdateCommand>(sale);
                return PartialView("_SalePaymentPartial", model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(SalePaymentUpdateCommand salePaymentUpdateCommand)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Invalid Input",
                        Type = ResponseType.Danger
                    });
                }
                else
                {
                    await _mediator.Send(salePaymentUpdateCommand);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Payment Completed Successfully",
                        Type = ResponseType.Success
                    });
                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Payment Failed");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Payment Failed",
                    Type = ResponseType.Danger
                });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> GetSaleJsonData([FromBody] SalesByQuery salesByQuery)
        {
            try
            {
                var (data, total, totalDisplay) = await _mediator.Send(salesByQuery);
                var idx = 0;
                var sales = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                                (++idx).ToString(),
                                record.Id.ToString(),
                                HttpUtility.HtmlEncode(record.SaleDate.ToString("dd/MM/yyyy")),
                                HttpUtility.HtmlEncode(record.CustomerName) + "<br/>" + HttpUtility.HtmlEncode(record.CustomerPhone),
                                HttpUtility.HtmlEncode(record.Total.ToString("N2")),
                                HttpUtility.HtmlEncode(record.Paid.ToString("N2")),
                                HttpUtility.HtmlEncode(record.Due.ToString("N2")),
                                record.PaymentStatus== PaymentStatus.Due? "Due":
                                record.PaymentStatus == PaymentStatus.Paid ? "Full Paid": "Partial Paid"
                            }).ToArray()
                };
                return Json(sales);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching sales data");
                return Json(SalesByQuery.EmptyResult);
            }
        }
    }
}
