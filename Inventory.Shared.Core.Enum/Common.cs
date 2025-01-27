using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Shared.Core.Enum
{
    public class Common
    {
        public enum SortDirection
        {
            Ascending = 0,
            Descending = 1
        }

        public enum DeleteStatus
        {
            NotDeleted = 0,
            Deleted = 1
        }

    }
}
