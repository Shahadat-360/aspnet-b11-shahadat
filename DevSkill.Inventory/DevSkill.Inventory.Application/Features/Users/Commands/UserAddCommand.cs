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
    public class UserAddCommand:IRequest
    {
        [Required(ErrorMessage = "Employee Name is required")]
        public string EmployeeId { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public Guid RoleId { get; set; }
        [Required(ErrorMessage = "Password is required"),
            StringLength(10,MinimumLength = 6,ErrorMessage = "Password must be between 6 and 10 characters")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public Status Status { get; set; } = Status.Active;
    }
}
