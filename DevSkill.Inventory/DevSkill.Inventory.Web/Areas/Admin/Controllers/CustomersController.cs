using DevSkill.Inventory.Application.Features.Customers.Commands;
using DevSkill.Inventory.Application.Features.Customers.Queries;
using DevSkill.Inventory.Domain.Enums;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomersController(ILogger<CustomersController> logger,IMediator mediator) : Controller
    {
        private readonly ILogger<CustomersController> _logger = logger;
        private readonly IMediator _mediator = mediator;
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            var model = new CustomerAddCommand();
            return PartialView("_CustomerAddModalPartial", model);
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CustomerAddCommand customerAddCommand)
        {
            try
            {
                customerAddCommand.CurrentBalance = customerAddCommand.OpeningBalance;
                if(ModelState.IsValid)
                {
                    await _mediator.Send(customerAddCommand);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Customer added successfully",
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
            catch(Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Error To Adding Customer",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error To Adding Customer");
            }
            return RedirectToAction("Index");
        }

        public async Task<JsonResult> GetCustomerJsonData([FromBody] CustomersByQuery model)
        {
            try
            {
                var (data, total, totalDisplay) = await _mediator.Send(model);
                int idx = 0;
                var customers = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                                (++idx).ToString(),
                                record.Id.ToString(),
                                HttpUtility.HtmlEncode(record.CustomerName),
                                HttpUtility.HtmlEncode(record.CompanyName),
                                HttpUtility.HtmlEncode(record.Mobile),
                                HttpUtility.HtmlEncode(record.Address),
                                HttpUtility.HtmlEncode(record.Email),
                                record.CurrentBalance.ToString("F2"),
                                record.Status==Status.Active ? "Active" : "Inactive",
                            })
                };
                return Json(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem in getting Customers");
                return Json(CustomersByQuery.EmptyResult);
            }
        }

    }
}
