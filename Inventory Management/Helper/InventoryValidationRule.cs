
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Inventory_Management.Helper
{
    public class InventoryValidationRule: ValidationRule
    {

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
           if(Int64.Parse( "0" + value.ToString()) <= 0)
            {
                return new ValidationResult(false, "Quantity must be greater than zero.");
            }
            return ValidationResult.ValidResult;
        }

     

    }
}
