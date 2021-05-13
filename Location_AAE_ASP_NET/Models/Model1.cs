using System.Data.Entity;

namespace Location_AAE_ASP_NET.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Droit_acces> droit_acces { get; set; }
        public virtual DbSet<Maintenance> maintenance { get; set; }
        public virtual DbSet<Parking> parking { get; set; }
        public virtual DbSet<Reservation> reservation { get; set; }
        public virtual DbSet<Utilisateur> utilisateur { get; set; }
        public virtual DbSet<Vehicule> vehicule { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Droit_acces>()
                .HasMany(e => e.utilisateur)
                .WithRequired(e => e.droit_acces)
                .HasForeignKey(e => e.accessRightsFK)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Maintenance>()
                .HasMany(e => e.vehicule)
                .WithRequired(e => e.maintenance)
                .HasForeignKey(e => e.maintenanceFK)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Parking>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Parking>()
                .Property(e => e.address)
                .IsUnicode(false);

            modelBuilder.Entity<Parking>()
                .HasMany(e => e.vehicule)
                .WithRequired(e => e.parking)
                .HasForeignKey(e => e.parkingFK)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Utilisateur>()
                .Property(e => e.entity)
                .IsUnicode(false);

            modelBuilder.Entity<Utilisateur>()
                .Property(e => e.connexionDate)
                .IsUnicode(false);

            modelBuilder.Entity<Utilisateur>()
                .Property(e => e.companyName)
                .IsUnicode(false);

            modelBuilder.Entity<Utilisateur>()
                .Property(e => e.companyPhone)
                .IsUnicode(false);

            modelBuilder.Entity<Utilisateur>()
                .Property(e => e.siret)
                .IsUnicode(false);

            modelBuilder.Entity<Utilisateur>()
                .Property(e => e.userLastName)
                .IsUnicode(false);

            modelBuilder.Entity<Utilisateur>()
                .Property(e => e.userFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Utilisateur>()
                .Property(e => e.userPhone)
                .IsUnicode(false);

            modelBuilder.Entity<Utilisateur>()
                .Property(e => e.userYearOld)
                .IsUnicode(false);

            modelBuilder.Entity<Utilisateur>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Utilisateur>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<Utilisateur>()
                .Property(e => e.licence)
                .IsUnicode(false);

            modelBuilder.Entity<Utilisateur>()
                .HasMany(e => e.reservation)
                .WithRequired(e => e.utilisateur)
                .HasForeignKey(e => e.utilisateurFK)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vehicule>()
                .Property(e => e.marque)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicule>()
                .Property(e => e.modele)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicule>()
                .HasMany(e => e.reservation)
                .WithRequired(e => e.vehicule)
                .HasForeignKey(e => e.vehiculeFK)
                .WillCascadeOnDelete(false);
        }
    }
}
