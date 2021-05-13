namespace Location_AAE_ASP_NET.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Droit_acces
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Droit_acces()
        {
            utilisateur = new HashSet<Utilisateur>();
        }

        [Key]
        public int droitAccesId { get; set; }

        [Required]
        [StringLength(50)]
        public string role { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Utilisateur> utilisateur { get; set; }
    }
}
