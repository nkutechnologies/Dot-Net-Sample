using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.ViewModels
{
    public class UpdateRoleViewModel
    {
        public UpdateRoleViewModel()
        {
            Users = new List<string>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Role Name is Required")]
        public string Name { get; set; }
        public List<string> Users { get; set; }
    }
}
