using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Customers.Commands
{
    public class CustomerUpdateCommand:IRequest
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Customer Name is required.")]
        public string CustomerName { get; set; }
        public string? CompanyName { get; set; }
        [Required(ErrorMessage = "Customer Mobile is required.")]
        public string Mobile { get; set; }
        public string? Address { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address."),
            Required(ErrorMessage = "Customer Email Is required")]
        public string Email { get; set; }
        public int OpeningBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public Status Status { get; set; }
    }
}
