using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.ViewModels.PackageViewModels
{
    public class PackageCRUPDlViewModel
    {
        public int PackageId { get; set; }
        [Required]
        public string PackageName { get; set; }
    }
}
