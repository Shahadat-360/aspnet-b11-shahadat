using DevSkill.Inventory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class BalanceTransferIndexDto
    {
        public Guid Id { get; set; }
        public DateTime TransferDate { get; set; }
        public AccountType SendingAccountType { get; set; }
        public string SendingAccount { get; set; }
        public AccountType ReceivingAccountType {  get; set; }
        public string ReceivingAccount { get; set; }
        public decimal TransferAmount { get; set; }
        public string? Note { get; set; }
    }
}
