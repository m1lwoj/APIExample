using Microsoft.EntityFrameworkCore;
using SnowDAL.DBModels;
using SnowDAL.Repositories.Concrete;
using SnowDAL.Repositories.Interfaces;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnowDAL.Searching;
using SnowDAL.Extensions;
using SnowDAL.Paging;
using System.Data;
using Npgsql;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SnowDAL.Concrete.Repositories
{
    public class RouteRepository : EntityBaseRepository<RouteInfoEntity>, IRouteRepository
    {
        public RouteRepository(EFContext context) : base(context)
        {
        }

        public async Task<RouteInfoEntity> GetSingleWithDependencies(int id)
        {
            return await this._context.Routes
                .Include(r => r.Geometry)
                .Include(r => r.User)
                .Include(r => r.Point)
                .FirstOrDefaultAsync(r => r.ID == id && r.Status == 1);
        }

        public async Task<IEnumerable<RouteGeomEntity>> GetGeometries(int[] ids)
        {
            return await this._context.RoutesGeom.Where(route => ids.Any(id => id == route.RouteInfo.ID && route.RouteInfo.Status == 1)).ToListAsync();
        }

        public async Task<PagingResult<RouteInfoEntity>> Search(SearchQuery<RouteInfoEntity> searchQuery)
        {
            IQueryable<RouteInfoEntity> sequence = this._context.Routes;

            sequence = ManageFilters(searchQuery.FiltersDictionary, sequence);

            sequence = ManageIncludeProperties(searchQuery.IncludeProperties, sequence);

            sequence = ManageSortCriterias(searchQuery.SortCriterias, sequence);

            return await GetTheResult(searchQuery, sequence);
        }

        public override void Delete(RouteInfoEntity entity)
        {
            entity.Geometry.Status = 0;
            EntityEntry geomEntity = _context.Entry(entity.Geometry);
            geomEntity.State = EntityState.Modified;

            entity.Point.Status = 0;
            EntityEntry pointEntity = _context.Entry(entity.Point);
            pointEntity.State = EntityState.Modified;

            base.Delete(entity);
        }
    }
}
