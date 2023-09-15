using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace battleship_server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        // The name of this DbSet is the name of the corresponding DB table
        public DbSet<Ship> Ships => Set<Ship>();
    }
}