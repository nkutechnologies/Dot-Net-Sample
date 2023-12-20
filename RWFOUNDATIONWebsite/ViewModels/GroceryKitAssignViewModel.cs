using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.ViewModels
{
    public class GroceryKitAssignViewModel
    {
        public int UserId { get; set; }
        public ICollection<Assign> Assign { get; set; }
    }
}
