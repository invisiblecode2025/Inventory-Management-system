using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;


namespace Inventory_Management.ViewModels
{
   public class ViewModelLocator
    {
        public ItemViewModel ItemViewModel
        => App.ServiceProvider.GetRequiredService<ItemViewModel>();


    }
}
