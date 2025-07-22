using DevSkill.Inventory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string? Password { get; set; }
        public Status Status { get; set; } = Status.Active;
    }
}
