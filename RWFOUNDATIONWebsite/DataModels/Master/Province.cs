using RWFOUNDATIONWebsite.DataModels.SaveAsDrafts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.DataModels.Master
{
    public class Province
    {
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public ICollection<GroceryKit> GroceryKits { get; set; }
        public ICollection<City> Cities { get; set; }
        public ICollection<BeneficiaryFormSaveAsDraft> BeneficiaryFormSaveAsDrafts { get; set; }

    }
}
