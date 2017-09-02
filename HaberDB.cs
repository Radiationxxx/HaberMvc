namespace HaberSitesi.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class HaberDB : DbContext
    {
        public HaberDB()
            : base("name=HaberDB")
        {
        }

        public virtual DbSet<Etiket> Etikets { get; set; }
        public virtual DbSet<Haber> Habers { get; set; }
        public virtual DbSet<Kategori> Kategoris { get; set; }
        public virtual DbSet<Kullanici> Kullanicis { get; set; }
        public virtual DbSet<Yetki> Yetkis { get; set; }
        public virtual DbSet<Yorum> Yorums { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Etiket>()
                .Property(e => e.EtiketAd)
                .IsFixedLength();

            modelBuilder.Entity<Etiket>()
                .HasMany(e => e.Habers)
                .WithMany(e => e.Etikets)
                .Map(m => m.ToTable("EtiketHaber").MapLeftKey("EtiketId").MapRightKey("HaberId"));

            modelBuilder.Entity<Haber>()
                .HasMany(e => e.Yorums)
                .WithRequired(e => e.Haber)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.Habers)
                .WithRequired(e => e.Kullanici)
                .HasForeignKey(e => e.OlusturanId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Yetki>()
                .Property(e => e.YetkiAd)
                .IsFixedLength();

            modelBuilder.Entity<Yetki>()
                .HasMany(e => e.Kullanicis)
                .WithRequired(e => e.Yetki)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Yorum>()
                .Property(e => e.YorumBaslik)
                .IsFixedLength();

            modelBuilder.Entity<Yorum>()
                .Property(e => e.YorumIcerik)
                .IsFixedLength();
        }
    }
}
