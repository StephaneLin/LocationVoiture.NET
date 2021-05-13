namespace Location_AAE_ASP_NET.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("utilisateur")]
    public partial class Utilisateur
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Utilisateur()
        {
            reservation = new HashSet<Reservation>();
        }

        [Key]
        public int userId { get; set; }

        [Required]
        [StringLength(50)]
        public string entity { get; set; }

        [Required]
        [StringLength(50)]
        public string connexionDate { get; set; }

        [Required]
        [StringLength(50)]
        public string companyName { get; set; }

        [Required]
        [StringLength(50)]
        public string companyPhone { get; set; }

        [Required]
        [StringLength(50)]
        public string siret { get; set; }

        [Required]
        [StringLength(50)]
        public string userLastName { get; set; }

        [Required]
        [StringLength(50)]
        public string userFirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string userPhone { get; set; }

        [Required]
        [StringLength(50)]
        public string userYearOld { get; set; }

        [Required]
        [StringLength(100)]
        public string email { get; set; }

        [Required]
        [StringLength(100)]
        public string password { get; set; }

        [Required]
        [StringLength(100)]
        public string licence { get; set; }

        public int accessRightsFK { get; set; }

        public virtual Droit_acces droit_acces { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation> reservation { get; set; }
    }
}
