using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlingIntegrationTagplus
{
    class IntegrationContext : DbContext
    {
        static readonly string DBFILE = "integration.db";
        readonly string DBCONNECTION = $"DataSource={DBFILE}";

        public DbSet<SettingString> SettingStrings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(DBCONNECTION);
    }

    public class SettingString
    {
        [Key]
        public string name { get; set; }
        [Required]
        public string value { get; set; }
    }

}
