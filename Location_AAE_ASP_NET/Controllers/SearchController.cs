using Location_AAE_ASP_NET.Models;
using Location_AAE_ASP_NET.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Location_AAE_ASP_NET.Controllers
{
    public class SearchController : Controller
    {
        private Model1 db = new Model1();

        // GET: Search
        public ActionResult Index(String searchString)
        {
            SearchViewModel search = new SearchViewModel();

            search.Parkings = (from park in db.parking select park).ToList();
            search.Users = (from user in db.utilisateur select user).ToList();
            search.Cars = (from car in db.vehicule select car).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                search.Cars = search.Cars.Where(x => x.marque.ToUpper().Contains(searchString.ToUpper()) ||
                                            x.modele.ToString().ToUpper().Contains(searchString.ToUpper()) ||
                                            x.annee.ToString().ToUpper().Contains(searchString.ToUpper()) ||
                                            x.kilometrage.ToString().ToUpper().Contains(searchString.ToUpper()) ||
                                            x.prix.ToString().ToUpper().Contains(searchString.ToUpper()));

                search.Parkings = search.Parkings.Where(x =>
                                            x.name.ToUpper().Contains(searchString.ToUpper()) ||
                                            x.address.ToUpper().Contains(searchString.ToUpper()) ||
                                            x.nbrCar.ToString().ToUpper().Contains(searchString.ToUpper()) ||
                                            x.phoneNumber.ToString().ToUpper().Contains(searchString.ToUpper()));

                search.Users = search.Users.Where(x =>
                                           x.email.ToUpper().Contains(searchString.ToUpper()) ||
                                           x.companyPhone.ToUpper().Contains(searchString.ToUpper()) ||
                                           x.connexionDate.ToUpper().Contains(searchString.ToUpper()) ||
                                           x.companyName.ToUpper().Contains(searchString.ToUpper()) ||
                                           x.siret.ToUpper().Contains(searchString.ToUpper()) ||
                                           x.userPhone.ToUpper().Contains(searchString.ToUpper()) ||
                                           x.userYearOld.ToUpper().Contains(searchString.ToUpper()) ||
                                           x.userLastName.ToUpper().Contains(searchString.ToUpper()) ||
                                           x.userFirstName.ToUpper().Contains(searchString.ToUpper()));

            }

            return View(search);
        }
    }
}