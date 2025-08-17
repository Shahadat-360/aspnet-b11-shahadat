using DevSkill.Inventory.Application.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Users.Commands
{
    public class UserUpdateCommandHandler(IUserService userService) : IRequestHandler<UserUpdateCommand>
    {
        private readonly IUserService _userService = userService;
        public async Task Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {
            await _userService.UpdateUserAsync(request);
        }
    }
}
