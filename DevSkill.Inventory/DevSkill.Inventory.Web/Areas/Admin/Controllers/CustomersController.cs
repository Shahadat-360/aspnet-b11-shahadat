using AutoMapper;
using DevSkill.Inventory.Application.Features.Categories.Queries;
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
    public class CustomersController(ILogger<CustomersController> logger,IMediator mediator,
        IMapper mapper) : Controller
    {
        private readonly ILogger<CustomersController> _logger = logger;
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            var model = new CustomerAddCommand();
            return PartialView("_CustomerModalPartial", model);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAsync(CustomerAddCommand customerAddCommand)
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

        public async Task<IActionResult> Update(string id)
        {
            var customer = await _mediator.Send(new CustomerGetByIdQuery { Id = id });
            var model = _mapper.Map<CustomerUpdateCommand>(customer);
            return PartialView("_CustomerModalPartial", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(CustomerUpdateCommand customerUpdateCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(customerUpdateCommand);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Customer updated successfully",
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
                    Message = "Error To Updating Customer",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error To Updating Customer");
            }
            return RedirectToAction("Index");
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string Id)
        {
            try
            {
                await _mediator.Send(new CustomerDeleteCommand { Id = Id });
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Customer deleted successfully",
                    Type = ResponseType.Success
                });
            }
            catch(Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Error To Deleting Customer",
                    Type = ResponseType.Danger
                });
                _logger.LogError(ex, "Error To Deleting Customer");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> SearchCustomers(string term = "", int page = 1, int pageSize = 5)
        {
            try
            {
                var result = await _mediator.Send(new CustomerSearchWithPaginationQuery { term = term, page = page, pageSize = pageSize });

                var results = result.Items.Select(c => new
                {
                    id = c.Id.ToString(),
                    text = c.CustomerName,
                    mobile = c.Mobile,
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
                _logger.LogError(ex, "Error searching Customers");
                return Json(new { results = new List<object>(), pagination = new { more = false } });
            }
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
