namespace Location_AAE_ASP_NET.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("reservation")]
    public partial class Reservation
    {
        public int reservationId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime debutDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime finDate { get; set; }

        public int status { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime reservationDate { get; set; }

        public int vehiculeFK { get; set; }

        public int utilisateurFK { get; set; }

        public virtual Utilisateur utilisateur { get; set; }

        public virtual Vehicule vehicule { get; set; }
    }
}
