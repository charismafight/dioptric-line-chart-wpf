using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SQLite.CodeFirst;

namespace Dioptric
{
    public class DioptricContext: DbContext
    {
        //定义属性，便于外部访问数据表
        public DbSet<DioptricModel> Dioptrics { get { return Set<DioptricModel>(); } }

        public DioptricContext() : base("dbConn")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ModelConfiguration.Configure(modelBuilder);
            var init = new SqliteDropCreateDatabaseWhenModelChanges<DioptricContext>(modelBuilder);
            //var init = new SqliteCreateDatabaseIfNotExists<MyBookDB>(modelBuilder);
            Database.SetInitializer(init);
        }
    }

    public class ModelConfiguration
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            ConfigureBookEntity(modelBuilder);
        }
        private static void ConfigureBookEntity(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DioptricModel>();
        }
    }
}
