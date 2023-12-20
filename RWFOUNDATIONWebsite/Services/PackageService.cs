using Microsoft.EntityFrameworkCore;
using RWFOUNDATIONWebsite.Data;
using RWFOUNDATIONWebsite.DataModels.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Services
{
    public class PackageService : IRepository<Package>
    {
        private readonly RwDbContext _context;
        public PackageService(RwDbContext context)
        {
            _context = context;
        }
        public void Add(ref Package entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public Task<bool> AddAsync(Package entity)
        {
            throw new NotImplementedException();
        }

        public bool AddRange(IEnumerable<Package> entity)
        {
            throw new NotImplementedException();
        }
        public int AddPackageDetail(ref PackageDetail entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
            return entity.PackageDetailId;
        }

        public bool Any(Package entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Package entity)
        {
            var found = _context.Packages.Where(x => x.PackageId == entity.PackageId).FirstOrDefault();
            found.IsActive = false;
            found.IsDeleted = true;
            _context.SaveChanges();
        }

        public Task<bool> DeleteAsync(Package entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteByFlag(Package entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteRange(Package entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(IEnumerable<Package> entity)
        {
            throw new NotImplementedException();
        }

        public Package Get(int id)
        {
            return _context.Packages.Where(x => x.PackageId == id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
        }

        public IEnumerable<Package> GetAll()
        {
            return _context.Packages.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
        }

        public PackageDetail GetPackageItem(int id)
        {
            return _context.PackageDetails
                .Include(x => x.Package)
                .Include(x => x.PackageItems)
                    .ThenInclude(x => x.Item)
                .Include(x => x.PackageItems)
                    .ThenInclude(x => x.Item)
                        .ThenInclude(x=>x.Unit)
                .Where(x => x.PackageDetailId == id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
        }

        public IEnumerable<PackageDetail> GetAllPackageItems()
        {
            return _context.PackageDetails
                .Include(x=>x.Package)
                .Include(x=>x.PackageItems)
                    .ThenInclude(x=>x.Item)
                .Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
        }

        public void Update(Package entity)
        {
            var found = _context.Packages.Where(x => x.PackageId == entity.PackageId).FirstOrDefault();
            found.PackageId = entity.PackageId;
            found.PackageName = entity.PackageName;
            found.UpdatedBy = entity.UpdatedBy;
            found.UpdatedOn = entity.UpdatedOn;
            _context.SaveChanges();
        }

        public Task<bool> UpdateAsync(Package entity)
        {
            throw new NotImplementedException();
        }
    }
}
