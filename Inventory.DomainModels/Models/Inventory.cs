using Inventory.DomainModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DomainModels.Models
{
    public partial class Inventory: BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Description { get; set; }    
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public  double ItemPrice { get; set; }
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public int StockQuantity { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
