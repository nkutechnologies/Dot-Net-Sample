using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.ViewModels.DonorViewModels
{
    public class GetValueToProcessViewModel
    {
        public int PackageId { get; set; }
        public int NoOfMonth { get; set;}
        public List<FormNoList> FormNos { get; set; }
    }
}
