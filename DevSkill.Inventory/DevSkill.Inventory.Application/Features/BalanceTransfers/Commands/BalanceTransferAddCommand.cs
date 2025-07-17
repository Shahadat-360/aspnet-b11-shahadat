using DevSkill.Inventory.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.BalanceTransfers.Commands
{
    public class BalanceTransferAddCommand:IRequest
    {
        [Required(ErrorMessage ="Transfer Date is Required")]
        public DateTime TransferDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Sending Account Type Must be Selected")]
        public AccountType SendingAccountType { get; set; } = AccountType.Cash;
        [Required(ErrorMessage ="Sending Account is Required")]
        public int SendingAccountId {  get; set; }
        [Required(ErrorMessage = "Receiving Account Type Must be Selected")]
        public AccountType ReceivingAccountType { get; set; } = AccountType.Cash;
        [Required(ErrorMessage = "Receiving Account is Required")]
        public int ReceivingAccountId { get; set; }
        [Range(1,double.MaxValue,ErrorMessage ="Amount Should be Greater Than Zero")]
        public decimal TransferAmount { get; set; }
        public string? Note { get; set; }
    }
}
