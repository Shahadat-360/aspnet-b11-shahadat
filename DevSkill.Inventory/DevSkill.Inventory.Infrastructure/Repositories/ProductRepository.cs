using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class ProductRepository(ApplicationDbContext dbContext): Repository<Product, Guid>(dbContext), IProductRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
    }
}
