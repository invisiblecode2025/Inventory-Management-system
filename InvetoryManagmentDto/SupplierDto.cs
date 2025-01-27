using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ManagementDto
{

    public record SupplierDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ContactInfo { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; }
        public ICollection<ItemDto> Items { get; set; }

    }
}
