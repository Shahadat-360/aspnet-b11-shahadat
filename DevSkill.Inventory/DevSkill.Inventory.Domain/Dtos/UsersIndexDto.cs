using DevSkill.Inventory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class UsersIndexDto
    {
        public Guid Id { get; set; }
        public string EmployeeName { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string RoleName { get; set; }
        public Status Status { get; set; }
    }
}
