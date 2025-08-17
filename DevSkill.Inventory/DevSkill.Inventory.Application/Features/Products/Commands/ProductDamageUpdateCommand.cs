using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Products.Commands
{
    public class ProductDamageUpdateCommand:IRequest
    {
        [Required(ErrorMessage = "Product ID is required.")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Damage Quantity is required."),
            Range(1, int.MaxValue, ErrorMessage = "Should be Greater Than 0")]
        public int Damage { get; set; }
    }
}
