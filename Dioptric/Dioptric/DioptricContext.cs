using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Dioptric
{
    public class PatientContext: DbContext
    {
        //定义属性，便于外部访问数据表
        public DbSet<PatientModel> Dioptrics { get { return Set<PatientModel>(); } }

        public PatientContext() : base("dbConn")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ModelConfiguration.Configure(modelBuilder);
            var init = new SqliteDropCreateDatabaseWhenModelChanges<PatientContext>(modelBuilder);
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
            modelBuilder.Entity<PatientModel>();
        }
    }
}
