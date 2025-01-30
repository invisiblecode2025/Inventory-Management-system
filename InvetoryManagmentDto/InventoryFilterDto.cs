using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Inventory.Shared.Core.Enum.Common;

namespace Inventory.ManagementDto
{
    public class InventoryFilterDto
    {

        public int SelectedStockStatus { get; set; }
        public int StatustSearch { get; set; }
        public StockStatusMinMax StockStatusMinMax { get; set; }
        public ExpressionType selectedExpressionType { get; set; }
        public int SelectedCategoryId { get; set; }

        public int SearchSelectedItemId { get; set; }
        public int SearchSelectedSupplierId { get; set; }

        public string? InputSearch { get; set; }
    }
}
