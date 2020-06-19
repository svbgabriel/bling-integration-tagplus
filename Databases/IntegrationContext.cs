using BlingIntegrationTagplus.Models;
using Microsoft.EntityFrameworkCore;

namespace BlingIntegrationTagplus.Databases
{
    public class IntegrationContext : DbContext
    {
        static readonly string DBFILE = "integration.db";
        readonly string DBCONNECTION = $"DataSource={DBFILE}";

        public DbSet<TagPlusToken> TagPlusTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(DBCONNECTION);
    }
}
