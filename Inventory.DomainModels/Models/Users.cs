using Inventory.DomainModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DomainModels.Models
{
    public class Users :BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? PassWord { get; set; }

    }
}
