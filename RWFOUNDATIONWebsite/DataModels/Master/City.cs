using RWFOUNDATIONWebsite.DataModels.SaveAsDrafts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.DataModels.Master
{
    public class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int ProvinceId { get; set; }
        public Province Province { get; set; }
        public ICollection<GroceryKit> GroceryKits { get; set; }
        public ICollection<BeneficiaryFormSaveAsDraft> BeneficiaryFormSaveAsDrafts { get; set; }
    }
}
