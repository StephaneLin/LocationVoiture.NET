namespace Location_AAE_ASP_NET.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("maintenance")]
    public partial class Maintenance
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Maintenance()
        {
            vehicule = new HashSet<Vehicule>();
        }

        [Key]
        public int PK_maintenanceId { get; set; }

        [Column(TypeName = "date")]
        public DateTime date { get; set; }

        public byte tire { get; set; }

        public byte framework { get; set; }

        public byte electronics { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vehicule> vehicule { get; set; }
    }
}
