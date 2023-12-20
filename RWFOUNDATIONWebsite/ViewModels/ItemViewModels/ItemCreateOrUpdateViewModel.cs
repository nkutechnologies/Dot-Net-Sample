using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.ViewModels.ItemViewModels
{
    public class ItemCreateOrUpdateViewModel
    {
        public int ItemId { get; set; }
        [Required]
        public string ItemName { get; set; }
        public string ItemNameUrdu { get; set; }
        [Required]
        public decimal Price { get; set; }        
        public int UnitId { get; set; }       
       
    }
}
