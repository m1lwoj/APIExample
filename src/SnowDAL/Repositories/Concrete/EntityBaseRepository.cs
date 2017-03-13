using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SnowDAL.DBModels;
using SnowDAL.Paging;
using SnowDAL.Repositories.Interfaces;
using SnowDAL.Searching;
using SnowDAL.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SnowDAL.Extensions;

namespace SnowDAL.Repositories.Concrete
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T>
    where T : class, IEntityBase, new()
    {
        protected EFContext _context;

        public EntityBaseRepository(EFContext context)
        {
            _context = context;
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return  _context.Set<T>().Where(x => x.Status == 1).AsEnumerable();
        }

        public T GetSingle(int id)
        {
            return _context.Set<T>().FirstOrDefault(x => x.ID == id && x.Status == 1);
        }

        public void Add(T entity)
        {
            entity.Status = 1;
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            _context.Set<T>().Add(entity);
        }

        public virtual void Update(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            entity.Status = 0;
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetSingle(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>()
                .Where(x => x.Status == 1)
                .FirstOrDefaultAsync(predicate);
        }
    }
}
