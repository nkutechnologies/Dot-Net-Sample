using RWFOUNDATIONWebsite.Data;
using RWFOUNDATIONWebsite.DataModels.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Services
{
    public class RoleService
    {
        private readonly RwDbContext _dbContext;       

        public RoleService(RwDbContext dbContext)
        {
            _dbContext = dbContext;           
        }

        public IEnumerable<ApplicationRole> GetAll()
        {
            IEnumerable<ApplicationRole> roles = _dbContext.AspNetRoles.ToList().OrderByDescending(s => s.Id);
            return roles;
        }

        public string GetUserRole(int userId)
        {
            string roleName = string.Empty;
            var userRole = _dbContext.UserRoles.Where(x => x.UserId == userId).FirstOrDefault();
            if (userRole != null)
                roleName = _dbContext.AspNetRoles.Where(x => x.Id == userRole.RoleId).Select(x => x.Name).FirstOrDefault().ToString();
            return roleName;
        }
    }
}
