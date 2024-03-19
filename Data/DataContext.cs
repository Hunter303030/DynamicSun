using DynamicSun.Models;
using Microsoft.EntityFrameworkCore;

namespace DynamicSun.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)  : base(options)
        {

        }

        public DbSet<Weather> Weather { get; set; }
    }
}
