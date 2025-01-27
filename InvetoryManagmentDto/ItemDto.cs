using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ManagementDto
{

    public record ItemDto
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }

        public DateTime CreatedDate { get; set; }
        public string? Notes { get; set; }
        public CategoryDto? Category { get; set; }

    }
}
