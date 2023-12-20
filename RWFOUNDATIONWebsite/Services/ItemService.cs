using Microsoft.EntityFrameworkCore;
using RWFOUNDATIONWebsite.Data;
using RWFOUNDATIONWebsite.DataModels.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Services
{
    public class ItemService : IRepository<Item>
    {
        private readonly RwDbContext _context;
        public ItemService(RwDbContext context)
        {
            _context = context;
        }
        public void Add(ref Item entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public Task<bool> AddAsync(Item entity)
        {
            throw new NotImplementedException();
        }

        public bool AddRange(IEnumerable<Item> entity)
        {
            throw new NotImplementedException();
        }

        public bool Any(Item entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Item entity)
        {
            var find = _context.Items.Include(x => x.Unit).Where(x => x.ItemId == entity.ItemId).FirstOrDefault();
            _context.Items.Remove(find);           
            _context.SaveChanges();
        }

        public Task<bool> DeleteAsync(Item entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteByFlag(Item entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteRange(Item entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(IEnumerable<Item> entity)
        {
            throw new NotImplementedException();
        }

        public Item Get(int id)
        {
            return _context.Items.Include(x => x.Unit).Where(x => x.ItemId == id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
        }

        public IEnumerable<Item> GetAll()
        {
            return _context.Items.Include(x => x.Unit).Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
        }

        public void Update(Item entity)
        {
            var find = _context.Items.Include(x=>x.Unit).Where(x => x.ItemId == entity.ItemId).FirstOrDefault();
            find.ItemId = entity.ItemId;
            find.ItemName = entity.ItemName;
            find.ItemNameUrdu = entity.ItemNameUrdu;
            find.Price = entity.Price;
            find.UnitId  = entity.UnitId;
            find.UpdatedBy = entity.UpdatedBy;
            find.UpdatedOn = entity.UpdatedOn;
            _context.SaveChanges();
        }

        public Task<bool> UpdateAsync(Item entity)
        {
            throw new NotImplementedException();
        }
    }
}
