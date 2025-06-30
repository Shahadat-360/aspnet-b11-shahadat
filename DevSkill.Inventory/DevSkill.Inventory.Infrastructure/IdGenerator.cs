using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure
{
    public class IdGenerator(ApplicationDbContext applicationDbContext) : IIdGenerator
    {
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
        public async Task<string> GenerateIdAsync(string Prefix)
        {
            using var transaction = _applicationDbContext.Database.BeginTransaction();
            var idTracker = await _applicationDbContext.IdTrackers.FirstOrDefaultAsync(t=> t.Prefix == Prefix);
            if (idTracker == null)
            {
                idTracker = new IdTracker
                {
                    Prefix = Prefix,
                    LastUsedNumber = 1
                };
                await _applicationDbContext.IdTrackers.AddAsync(idTracker);
            }
            else
            {
                idTracker.LastUsedNumber++;
                _applicationDbContext.IdTrackers.Update(idTracker);
            }
            await _applicationDbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return $"{Prefix}{idTracker.LastUsedNumber:D5}";
        }
    }
}
