using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EasyArchitect.OutsideManaged.AuthExtensions.Models
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accountvo> Accountvos { get; set; } = null!;

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseOracle("Data Source=172.20.82.190:1521/orcl.mshome.net;User Id=system;Password=GELISpass01!");
//            }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("USING_NLS_COMP");

            modelBuilder.Entity<Accountvo>(entity =>
            {
                entity.HasKey(e => e.Userid);

                entity.ToTable("ACCOUNTVO");

                entity.Property(e => e.Userid)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("USERID");

                entity.Property(e => e.Password)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");
            });

            modelBuilder.HasSequence("LOGMNR_DIDS$");

            modelBuilder.HasSequence("LOGMNR_EVOLVE_SEQ$");

            modelBuilder.HasSequence("LOGMNR_SEQ$");

            modelBuilder.HasSequence("LOGMNR_UIDS$").IsCyclic();

            modelBuilder.HasSequence("MVIEW$_ADVSEQ_GENERIC");

            modelBuilder.HasSequence("MVIEW$_ADVSEQ_ID");

            modelBuilder.HasSequence("ROLLING_EVENT_SEQ$");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
