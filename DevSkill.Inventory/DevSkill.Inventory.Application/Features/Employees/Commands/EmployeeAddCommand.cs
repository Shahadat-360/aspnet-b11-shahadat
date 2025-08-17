using DevSkill.Inventory.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Employees.Commands
{
    public class EmployeeAddCommand:IRequest
    {
        [Required(ErrorMessage = "Employee Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Department is required.")]
        public int DepartmentId { get; set; }
        public string? Address { get; set; }
        [Required(ErrorMessage = "Employee Mobile is required.")]
        public string Mobile { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address."),
            Required(ErrorMessage ="Employee Email Is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Joining Date is required.")]
        public DateTime JoiningDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage ="Salary is required.")
            ,Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
        public decimal? Salary { get; set; }
        public string? NationalId { get; set; }
        [Required(ErrorMessage = "Status is required.")]
        public Status Status { get; set; } = Status.Active;
    }
}
