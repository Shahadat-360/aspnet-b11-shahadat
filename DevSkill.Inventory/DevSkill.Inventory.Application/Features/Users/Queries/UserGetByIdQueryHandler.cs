using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Users.Queries
{
    internal class UserGetByIdQueryHandler(IUserService userService) : IRequestHandler<UserGetByIdQuery, UserDto>
    {
        private readonly IUserService _userService = userService;
        public async Task<UserDto> Handle(UserGetByIdQuery request, CancellationToken cancellationToken)
        {
            return await _userService.UserByIdAsync(request.Id);
        }
    }
}
