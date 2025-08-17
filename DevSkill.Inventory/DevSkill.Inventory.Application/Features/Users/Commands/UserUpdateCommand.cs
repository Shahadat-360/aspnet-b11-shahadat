using DevSkill.Inventory.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Users.Commands
{
    public class UserUpdateCommand:IRequest
    {
        public Guid Id { get; set; }
        public string EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public Guid RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? Password { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public Status Status { get; set; } = Status.Active;
    }
}
