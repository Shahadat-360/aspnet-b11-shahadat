using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class ProductSearchDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public decimal? MaxPurchasePrice {  get; set; }
        public decimal? MinPurchasePrice {  get; set; }
        public decimal? MaxMRP { get; set; }
        public decimal? MinMRP { get; set; }
        public decimal? MaxWholesalePrice {  get; set; }
        public decimal? MinWholesalePrice {  get; set; }
    }
}