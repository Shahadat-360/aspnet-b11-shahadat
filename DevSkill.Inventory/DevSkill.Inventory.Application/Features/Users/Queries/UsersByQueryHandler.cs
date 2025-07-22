using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Users.Queries
{
    public class UsersByQueryHandler(IApplicationUnitOfWork applicationUnitOfWork):IRequestHandler<UsersByQuery, (IList<UsersIndexDto> Users, int total, int totalDisplay)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<(IList<UsersIndexDto> Users, int total, int totalDisplay)> Handle(UsersByQuery request, CancellationToken cancellationToken)
        {
        string? order = request.FormatSortExpression("ID","EmployeeName","Company", "Email", "Mobile", "RoleName", "Status");
            return await _applicationUnitOfWork.GetPagedUsers(request.PageIndex, request.PageSize, order, request.SearchItem);
        }
    }
}
