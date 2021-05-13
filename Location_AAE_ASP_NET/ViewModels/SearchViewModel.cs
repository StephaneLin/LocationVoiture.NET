using Location_AAE_ASP_NET.Models;
using System.Collections.Generic;

namespace Location_AAE_ASP_NET.ViewModels
{
    public class SearchViewModel
    {
        public IEnumerable<Parking> Parkings { get; set; }
        public IEnumerable<Utilisateur> Users { get; set; }
        public IEnumerable<Vehicule> Cars { get; set; }
    }
}