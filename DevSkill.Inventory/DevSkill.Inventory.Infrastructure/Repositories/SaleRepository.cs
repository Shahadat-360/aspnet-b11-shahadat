using AutoMapper;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
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
    public class SaleRepository(ApplicationDbContext applicationDbContext,
        IIdGenerator idGenerator,IMapper mapper)
        : Repository<Sale, string>(applicationDbContext), ISaleRepository
    {
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly IMapper _mapper = mapper;
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
        public async Task<Sale> GetSaleByIdWithNavigationAsync(string id)
        {
            var sales = await GetAsync(
                s => s.Id == id,
                q => q.Include(s => s.SaleItems)
                        .ThenInclude(p=>p.Product)
                            .ThenInclude(u=>u.Unit)
                      .Include(s=>s.Customer));
            return sales.Single();
        }

        public async Task<Sale> UpdateSaleWithItemsAsync(Sale sale, IList<SaleItemDto> newSaleItems)
        {
            var existingItems = sale.SaleItems.ToList();
            var existingItemsById = existingItems.ToDictionary(si => si.Id, si => si);

            var newIds = newSaleItems
                .Where(d => !string.IsNullOrEmpty(d.Id))
                .Select(d => d.Id)
                .ToHashSet();

            var toRemove = existingItems
                .Where(si => !newIds.Contains(si.Id))
                .ToList();

            foreach (var si in toRemove)
            {
                var product = await _applicationDbContext.Products
                                      .FindAsync(si.ProductId);
                if (product != null)
                {
                    product.Stock += si.Quantity;
                    _applicationDbContext.Products.Update(product);
                }
                sale.SaleItems.Remove(si);
            }

            foreach (var dto in newSaleItems.Where(d => !string.IsNullOrEmpty(d.Id)))
            {
                if (!existingItemsById.TryGetValue(dto.Id, out var existing))
                {
                    throw new InvalidOperationException($"Sale item with ID {dto.Id} not found");
                }

                if (existing.Quantity != dto.Quantity)
                {
                    var diff = existing.Quantity - dto.Quantity;
                    var product = await _applicationDbContext.Products
                                          .FindAsync(existing.ProductId);
                    if (product != null)
                    {
                        if (diff < 0 && product.Stock < Math.Abs(diff))
                        {
                            throw new InvalidOperationException(
                                $"Insufficient stock for product {existing.ProductId}. Available: {product.Stock}, Required: {Math.Abs(diff)}");
                        }

                        product.Stock += diff;
                        _applicationDbContext.Products.Update(product);
                    }
                }

                _mapper.Map(dto, existing);
            }

            foreach (var dto in newSaleItems.Where(d => string.IsNullOrEmpty(d.Id)))
            {
                var product = await _applicationDbContext.Products
                                      .FindAsync(dto.ProductId);
                if (product != null)
                {
                    if (product.Stock < dto.Quantity)
                        throw new InvalidOperationException(
                            $"Insufficient stock for product {dto.ProductId}. Available: {product.Stock}, Required: {dto.Quantity}");

                    product.Stock -= dto.Quantity;
                    _applicationDbContext.Products.Update(product);
                }
                else
                {
                    throw new InvalidOperationException($"Product with ID {dto.ProductId} not found");
                }

                var newItem = new SaleItem();
                _mapper.Map(dto, newItem);
                newItem.Id = await _idGenerator.GenerateIdAsync("Inv-Dev-Item");
                newItem.SaleId = sale.Id;
                sale.SaleItems.Add(newItem);
            }
            return sale;
        }

        public void ProductQuantityRestored(Sale sale)
        {
            foreach (var saleItem in sale.SaleItems)
            {
                if (saleItem.Product != null)
                {
                    saleItem.Product.Stock += saleItem.Quantity;
                }
            }
        }
    }
}
