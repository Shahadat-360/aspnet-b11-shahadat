using DevSkill.Inventory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class BalanceTransfer:IEntity<Guid>
    {
        public Guid Id { get; set; }
        public DateTime TransferDate;
        public AccountType SendingAccountType {  get; set; }
        public int SendingAccountId { get; set; }
        public decimal TransferAmount { get; set; }
        public AccountType ReceivingAccountType {  get; set; }
        public int ReceivingAccountId { get; set; }
        public string Note { get; set; }
    }
}
