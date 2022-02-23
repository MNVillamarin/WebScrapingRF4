using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Scraping.Models
{
    public partial class RF4RecordsContext : DbContext
    {
        public RF4RecordsContext()
        {
        }

        public RF4RecordsContext(DbContextOptions<RF4RecordsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Fish> Fish { get; set; }
        public virtual DbSet<FishLocation> FishLocations { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Registro> Registros { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-CS36O64; Database=RF4Records; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Fish>(entity =>
            {
                entity.Property(e => e.FishName)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FishLocation>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("FishLocation");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.IdFishNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdFish)
                    .HasConstraintName("FK_FishLocation_Fish");

                entity.HasOne(d => d.IdLocationNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdLocation)
                    .HasConstraintName("FK_FishLocation_Location");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.LocationName)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("Region");

                entity.Property(e => e.RegionName)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RegionUrl)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Registro>(entity =>
            {
                entity.HasIndex(e => new { e.Region, e.Fish, e.Weight, e.Location, e.Lure, e.Player, e.Fecha }, "uniqueindex")
                    .IsUnique();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.Property(e => e.Lure)
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.Player)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Weight)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.HasOne(d => d.FishNavigation)
                    .WithMany(p => p.Registros)
                    .HasForeignKey(d => d.Fish)
                    .HasConstraintName("FK_Registros_Fish");

                entity.HasOne(d => d.LocationNavigation)
                    .WithMany(p => p.Registros)
                    .HasForeignKey(d => d.Location)
                    .HasConstraintName("FK_Registros_Location");

                entity.HasOne(d => d.RegionNavigation)
                    .WithMany(p => p.Registros)
                    .HasForeignKey(d => d.Region)
                    .HasConstraintName("FK_Registros_Region");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
