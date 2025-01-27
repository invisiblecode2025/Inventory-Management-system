using Inventory.Shared.Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DomainModels.Base
{
    public class BaseEntity
    {


        internal BaseEntity()
        {
            DeleteStatus = (int)Common.DeleteStatus.NotDeleted;
        }

        [Required]
        public int DeleteStatus { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastUpdateDate { get; set; }

    }
}
