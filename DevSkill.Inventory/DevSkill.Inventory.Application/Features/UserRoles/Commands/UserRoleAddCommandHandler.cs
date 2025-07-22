using AutoMapper;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.UserRoles.Commands
{
    public class UserRoleAddCommandHandler(IRoleService roleService,IMapper mapper) 
        : IRequestHandler<UserRoleAddCommand>
    {
        private readonly IRoleService _roleService = roleService;
        private readonly IMapper _mapper = mapper;
        public async Task Handle(UserRoleAddCommand request, CancellationToken cancellationToken)
        {   
            await _roleService.AddRoleAsync(request);
        }
    }
}
