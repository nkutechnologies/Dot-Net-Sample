using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.DataModels.UserManagement
{
    public class ApplicationUserRole: IdentityUserRole<int>
    {
        public ApplicationRole ApplicationRole { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
