namespace Location_AAE_ASP_NET.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("vehicule")]
    public partial class Vehicule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vehicule()
        {
            reservation = new HashSet<Reservation>();
        }

        public int vehiculeId { get; set; }

        [Required]
        [StringLength(50)]
        public string marque { get; set; }

        [Required]
        [StringLength(50)]
        public string modele { get; set; }

        public int annee { get; set; }

        public int kilometrage { get; set; }

        [Required]
        [StringLength(50)]
        public string photo { get; set; }

        public int prix { get; set; }

        public int? maintenanceFK { get; set; }

        public int parkingFK { get; set; }

        public virtual Maintenance maintenance { get; set; }

        public virtual Parking parking { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation> reservation { get; set; }
    }
}
