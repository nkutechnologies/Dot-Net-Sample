using Microsoft.EntityFrameworkCore;
using RWFOUNDATIONWebsite.Data;
using RWFOUNDATIONWebsite.DataModels.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Services
{
    public class GrocerykitService : IRepository<GroceryKit>, IDisposable
    {
        private readonly RwDbContext _context;
        public GrocerykitService(RwDbContext context)
        {
            _context = context;
        }
        public void Add(ref GroceryKit entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }
        public int AddGrocerykit(ref GroceryKit entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
            return entity.GroceryKitId;
        }

        public Task<bool> AddAsync(GroceryKit entity)
        {
            throw new NotImplementedException();
        }

        public bool AddRange(IEnumerable<GroceryKit> entity)
        {
            throw new NotImplementedException();
        }

        public bool Any(GroceryKit entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(GroceryKit entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(GroceryKit entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteByFlag(GroceryKit entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteRange(GroceryKit entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(IEnumerable<GroceryKit> entity)
        {
            throw new NotImplementedException();
        }

        public GroceryKit Get(int id)
        {
            return _context.GroceryKits
                .Include(x => x.FamilyMembers)
                    .ThenInclude(x => x.Relation)
                .Include(x=>x.FamilyMembers)
                    .ThenInclude(x=>x.FamilyMemberStatus)
                .Include(x=>x.Occupation)
                .Include(x=>x.MedicalProb)
                .Include(x=>x.CurrentStatus)               
                .Include(x=>x.Province)
                .Include(x=>x.City)
                .Where(x =>x.GroceryKitId == id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
        }
        public GroceryKit GetBeneficiary(int id)
        {
            return _context.GroceryKits
                .Include(x => x.FamilyMembers)
                    .ThenInclude(x => x.Relation)
                .Include(x => x.FamilyMembers)
                    .ThenInclude(x => x.FamilyMemberStatus)
                .Include(x => x.Occupation)
                .Include(x => x.MedicalProb)
                .Include(x => x.CurrentStatus)
                .Include(x => x.Province)
                .Include(x => x.City)
                .Where(x => x.DataCollectedById == id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
        }

        public IEnumerable<GroceryKit> GetAll()
        {
            return _context.GroceryKits
                .Include(x=>x.FamilyMembers)
                    .ThenInclude(x=>x.Relation)
                .Include(x => x.FamilyMembers)
                    .ThenInclude(x => x.FamilyMemberStatus)
                .Include(x => x.Occupation)
                .Include(x => x.MedicalProb)
                .Include(x => x.CurrentStatus)
                .Include(x => x.Province)
                .Include(x => x.City)             
                .Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
        }
        public IEnumerable<GroceryKit> GetAllAppeal()
        {
            return _context.GroceryKits.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
        }
        public IEnumerable<GroceryKitAssign> Getallgrocerykitfordonor(int UserId)
        {
            return _context.GroceryKitAssigns
                .Include(x => x.GroceryKit)
                    .ThenInclude(x=>x.FamilyMembers)
                        .ThenInclude(x=>x.Relation)
                .Include(x => x.GroceryKit)
                    .ThenInclude(x => x.FamilyMembers)
                        .ThenInclude(x => x.FamilyMemberStatus)
                .Include(x => x.GroceryKit)
                    .ThenInclude(x => x.Occupation)
                .Include(x => x.GroceryKit)
                    .ThenInclude(x => x.MedicalProb)
                .Include(x => x.GroceryKit)
                    .ThenInclude(x => x.CurrentStatus)
                .Include(x => x.GroceryKit)
                    .ThenInclude(x => x.Province)
                .Include(x => x.GroceryKit)
                    .ThenInclude(x => x.City)
                .Include(x => x.ApplicationUser)
                .Where(x =>x.UserId == UserId && String.IsNullOrEmpty(x.GroceryKit.SponsorStatus) && x.IsActive == true && x.IsDeleted == false).ToList();
        }
        public void Update(GroceryKit entity)
        {
            _context.GroceryKits.Update(entity);
            _context.SaveChanges();
        }

        public Task<bool> UpdateAsync(GroceryKit entity)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
