using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace PennyTest2.DataBase
{
    public partial class PennyTest_Entities : DbContext
    {
        public PennyTest_Entities()
            : base("name=PennyTest_Entities1")
        {
        }

        public virtual DbSet<employee> employee { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Task> task { get; set; }
        public virtual DbSet<TokenEmp> TokenEmp { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<employee>()
                .Property(e => e.EMPNO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<employee>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<employee>()
                .Property(e => e.EMPPASS)
                .IsUnicode(false);

            modelBuilder.Entity<employee>()
                .HasMany(e => e.TokenEmp)
                .WithRequired(e => e.employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Task>()
                .Property(e => e.EMPNO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .Property(e => e.TASKID)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .Property(e => e.MEMO)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .Property(e => e.STATE)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
               .Property(e => e.SERIALNO)
               .IsUnicode(false);

            modelBuilder.Entity<TokenEmp>()
                .Property(e => e.EMPNO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TokenEmp>()
                .Property(e => e.ACCESSTOKEN)
                .IsUnicode(false);

            modelBuilder.Entity<TokenEmp>()
                .Property(e => e.CREATOR)
                .IsUnicode(false);

            modelBuilder.Entity<TokenEmp>()
                .Property(e => e.MODIFER)
                .IsUnicode(false);
        }
    }
}
