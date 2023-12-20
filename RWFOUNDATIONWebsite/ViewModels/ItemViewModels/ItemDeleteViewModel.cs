using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.ViewModels.ItemViewModels
{
    public class ItemDeleteViewModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemNameUrdu { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
    }
}
