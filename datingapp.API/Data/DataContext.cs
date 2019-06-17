using datingapp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace datingapp.API.Data
{
    public class DataContext : DbContext
    {
         public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Value> Values { get; set; }

        public DbSet<User> User {get; set;}

        public DbSet<Photo> Photos { get; set; }

        
    }
}