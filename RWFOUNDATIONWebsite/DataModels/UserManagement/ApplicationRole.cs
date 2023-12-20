using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.DataModels.UserManagement
{
    public class ApplicationRole: IdentityRole<int>
    {
        public ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; }
    }
}
