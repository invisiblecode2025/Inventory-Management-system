using Inventory.DomainModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DomainModels.Models
{
    public class Supplier :BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ContactInfo { get; set; }
        public string? Email { get; set; }

        public string? Description { get; set; }
    } 
    
}
