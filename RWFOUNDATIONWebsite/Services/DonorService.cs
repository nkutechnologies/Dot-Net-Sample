using Microsoft.EntityFrameworkCore;
using RWFOUNDATIONWebsite.Data;
using RWFOUNDATIONWebsite.DataModels.Donors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Services
{
    public class DonorService: IRepository<DonorRequestForBeneficiary>
    {
        private readonly RwDbContext _context;
        public DonorService(RwDbContext context)
        {
            _context = context;
        }

        public void Add(ref DonorRequestForBeneficiary entity)
        {
            throw new NotImplementedException();
        }
        public int Create(ref DonorRequestForBeneficiary entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }

        public Task<bool> AddAsync(DonorRequestForBeneficiary entity)
        {
            throw new NotImplementedException();
        }

        public bool AddRange(IEnumerable<DonorRequestForBeneficiary> entity)
        {
            throw new NotImplementedException();
        }

        public bool Any(DonorRequestForBeneficiary entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(DonorRequestForBeneficiary entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(DonorRequestForBeneficiary entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteByFlag(DonorRequestForBeneficiary entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteRange(DonorRequestForBeneficiary entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(IEnumerable<DonorRequestForBeneficiary> entity)
        {
            throw new NotImplementedException();
        }

        public DonorRequestForBeneficiary Get(int id)
        {
            return _context.DonorRequestForBeneficiaries.Find(id);
        }

        public IEnumerable<DonorRequestForBeneficiary> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DonorRequestForBeneficiary> GetRequestsByUserId(int id)
        {
            return _context.DonorRequestForBeneficiaries
                .Include(x=>x.ApplicationUser)
                .Include(x=>x.FamilyMemberForDonors)
                .Include(x=>x.DonationType)
                .Where(x => x.IsActive == true && x.IsDeleted == false && x.RequestTo == id).OrderByDescending(p => p.Id).ToList();
        }
        public IEnumerable<DonorRequestForBeneficiary> GetunreadRequestByUserId(int id)
        {
            return _context.DonorRequestForBeneficiaries.Include(x => x.ApplicationUser)
                .Include(x => x.FamilyMemberForDonors)
                .Where(x => x.IsActive == true && x.IsDeleted == false && x.RequestTo == id && x.IsRead == false).OrderByDescending(p => p.Id).ToList();
        }
        public void Update(DonorRequestForBeneficiary entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(DonorRequestForBeneficiary entity)
        {
            throw new NotImplementedException();
        }
        public void UpdateRange(IEnumerable<DonorRequestForBeneficiary> entity)
        {
            _context.DonorRequestForBeneficiaries.UpdateRange(entity);
            _context.SaveChanges();
        }
    }
}
