using DevSkill.Inventory.Application.Features.BalanceTransfers.Commands;
using DevSkill.Inventory.Application.Features.BalanceTransfers.Queries;
using DevSkill.Inventory.Application.Features.Sales.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Enums;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Policy = Permissions.BalanceTransferPage)]
    public class BalanceTransfersController(IMediator mediator,ILogger<BalanceTransfersByQuery> logger) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<BalanceTransfersByQuery> _logger = logger;
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet,Authorize(Policy = Permissions.BalanceTransferAdd)]
        public IActionResult Add()
        {
            var model = new BalanceTransferAddCommand();
            return PartialView("_BalanceTransferModalPartial",model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = Permissions.BalanceTransferAdd)]
        public async Task<IActionResult> Add(BalanceTransferAddCommand balanceTransferAddCommand)
        {
            try
            {
                await _mediator.Send(balanceTransferAddCommand);
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Balance Tranfered SuccessFully",
                    Type = ResponseType.Success,
                });
                return RedirectToAction("Index");
            }
            catch(InvalidOperationException ioe)
            {
                _logger.LogError(ioe.Message, "Invalid Operation In Adding Balance Transfer");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = ioe.Message,
                    Type = ResponseType.Danger,
                });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var message = "Balance Transfer Failed";
                _logger.LogError(ex, message);
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = message,
                    Type = ResponseType.Danger,
                });
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetBankTransferJsonData([FromBody] BalanceTransfersByQuery balanceTransfersByQuery)
        {
            try
            {
                var (data, total, totalDisplay) = await _mediator.Send(balanceTransfersByQuery);
                var idx = 0;
                var BalanceTransfers = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                                (++idx).ToString(),
                                HttpUtility.HtmlEncode(record.TransferDate.ToString("dd/MM/yyyy")),
                                HttpUtility.HtmlEncode(record.SendingAccountType.ToString()),
                                HttpUtility.HtmlEncode(record.SendingAccount.ToString()),
                                HttpUtility.HtmlEncode(record.ReceivingAccountType.ToString()),
                                HttpUtility.HtmlEncode(record.ReceivingAccount.ToString()),
                                HttpUtility.HtmlEncode(record.TransferAmount.ToString("N2")),
                                HttpUtility.HtmlEncode(string.IsNullOrEmpty(record.Note)?string.Empty:record.Note),
                                HttpUtility.HtmlEncode(record.Id.ToString())
                            }).ToArray()
                };
                return Json(BalanceTransfers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching balance transfer data");
                return Json(BalanceTransfersByQuery.EmptyResult);
            }
        }

        [HttpPost,ValidateAntiForgeryToken, Authorize(Policy = Permissions.BalanceTransferDelete)]
        public async Task<IActionResult> Delete(Guid Id)
        {
            try
            {
                if (Id == Guid.Empty)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Balance Tranfer Id Not Found",
                        Type = ResponseType.Danger,
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    await _mediator.Send(new BalanceTransferDeleteCommand { Id = Id });
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Balance Transfer Record Deleted SuccessFully",
                        Type = ResponseType.Success,
                    });
                    return RedirectToAction("Index");
                }
            }
            catch(InvalidOperationException ioe)
            {
                _logger.LogError(ioe.Message, "Invalid Operation In Delete");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = ioe.Message.ToString(),
                    Type = ResponseType.Danger,
                });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var message = "Failed To Delete Balance Transfer Record";
                _logger.LogError(ex, message);
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = message,
                    Type = ResponseType.Danger,
                });
                return RedirectToAction("Index");
            }
        }
    }
}
