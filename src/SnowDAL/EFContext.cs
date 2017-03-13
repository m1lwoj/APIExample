using Microsoft.EntityFrameworkCore;
using SnowDAL.DBModels;

namespace SnowDAL
{
    public class EFContext : DbContext
    {
        public EFContext(DbContextOptions<EFContext> options) : base(options)
        {
        }

        public DbSet<RouteInfoEntity> Routes { get; set; }
        public DbSet<RouteGeomEntity> RoutesGeom { get; set; }
        public DbSet<RoutePointEntity> RoutesPoint { get; set; }
    }
}
