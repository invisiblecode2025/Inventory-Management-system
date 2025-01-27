using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ManagementDto
{

    public record InventoryDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int ItemId { get; set; }
        public ItemDto Item { get; set; }
        public decimal ItemPrice { get; set; }
        public int SupplierId { get; set; }
        public SupplierDto Supplier { get; set; }
        public int StockQuantity { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
