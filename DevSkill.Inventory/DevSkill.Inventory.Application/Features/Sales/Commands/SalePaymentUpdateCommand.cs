using DevSkill.Inventory.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Sales.Commands
{
    public class SalePaymentUpdateCommand : IRequest
    {
        public string Id { get; set; }
        public decimal Due { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Paid amount must be between 1 and the Due amount.")]
        public decimal Paid { get; set; }
        public string? AccountName { get; set; }
        public AccountType AccountType { get; set; } = AccountType.Cash;
        public int AccountId { get; set; }
        public string? Note { get; set; }
    }
}
