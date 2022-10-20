using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Huellero.Models
{
    public partial class PruebaContext : DbContext
    {
        public PruebaContext()
        {
        }

        public PruebaContext(DbContextOptions<PruebaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblUsuario> TblUsuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("server=(localdb)\\MSSQLLocalDB; database=Prueba; integrated security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblUsuario>(entity =>
            {
                entity.ToTable("TBL_USUARIO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.VApellidos)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("V_APELLIDOS");

                entity.Property(e => e.VHuella)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("V_HUELLA");

                entity.Property(e => e.VNombres)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("V_NOMBRES");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
