using Microsoft.EntityFrameworkCore;
using RWFOUNDATIONWebsite.Data;
using RWFOUNDATIONWebsite.DataModels.SaveAsDrafts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Services
{
    public class BeneficiarySaveAsDraftService : IRepository<BeneficiaryFormSaveAsDraft>
    {
        private readonly RwDbContext _context;
        public BeneficiarySaveAsDraftService(RwDbContext context)
        {
            _context = context;
        }
        public void Add(ref BeneficiaryFormSaveAsDraft entity)
        {
            throw new NotImplementedException();
        }
        public int SaveDraft(ref BeneficiaryFormSaveAsDraft entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
            return entity.GroceryKitId;
        }

        public Task<bool> AddAsync(BeneficiaryFormSaveAsDraft entity)
        {
            throw new NotImplementedException();
        }

        public bool AddRange(IEnumerable<BeneficiaryFormSaveAsDraft> entity)
        {
            throw new NotImplementedException();
        }

        public bool Any(BeneficiaryFormSaveAsDraft entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(BeneficiaryFormSaveAsDraft entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(BeneficiaryFormSaveAsDraft entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteByFlag(BeneficiaryFormSaveAsDraft entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteRange(BeneficiaryFormSaveAsDraft entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(IEnumerable<BeneficiaryFormSaveAsDraft> entity)
        {
            throw new NotImplementedException();
        }

        public BeneficiaryFormSaveAsDraft Get(int id)
        {
            return _context.BeneficiaryFormSaveAsDrafts
               .Include(x => x.FamilyMembers)
                    .ThenInclude(x => x.Relation)
                .Include(x => x.FamilyMembers)
                    .ThenInclude(x => x.FamilyMemberStatus)
                .Include(x => x.Occupation)
                .Include(x => x.MedicalProb)
                .Include(x => x.CurrentStatus)
                .Include(x => x.Province)
                .Include(x => x.City)
                .Where(x => x.CreatedBy == id).FirstOrDefault();
        }

        public IEnumerable<BeneficiaryFormSaveAsDraft> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(BeneficiaryFormSaveAsDraft entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(BeneficiaryFormSaveAsDraft entity)
        {
            throw new NotImplementedException();
        }
    }
}
