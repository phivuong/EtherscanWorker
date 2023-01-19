using EtherscanWorker.Entities;
using Microsoft.EntityFrameworkCore;

namespace EtherscanWorker.Helpers
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server with connection string from app settings
            options.UseSqlServer(Configuration.GetConnectionString("EtherscanDatabase"));
        }

        public DbSet<Token> Token { get; set; }
    }
}