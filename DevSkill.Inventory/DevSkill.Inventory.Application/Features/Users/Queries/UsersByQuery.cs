using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Users.Queries
{
    public class UsersByQuery:DataTables,IRequest<(IList<UsersIndexDto>,int,int)>
    {
        public UserSearchDto? SearchItem { get; set; }
    }
}
