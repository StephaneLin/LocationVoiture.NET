using Location_AAE_ASP_NET.Infrastructure;
using Location_AAE_ASP_NET.Models;
using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Location_AAE_ASP_NET.Controllers
{
    [AuthenticationFilter]
    [AutorizationLevelFilter(2)]
    public class VehiculesController : Controller
    {
        private Model1 db = new Model1();

        // GET: vehicules
        public ActionResult Index(string sortOrder, string search, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortingId = String.IsNullOrEmpty(sortOrder) ? "Desc parkingId" : "";
            ViewBag.SortingMarque = sortOrder == "Marque" ? "Desc marque" : "Marque";
            ViewBag.SortingModele = sortOrder == "Modele" ? "Desc modele" : "Modele";
            ViewBag.SortingYear = sortOrder == "Annee" ? "Desc annee" : "Annee";
            ViewBag.SortingKm = sortOrder == "Km" ? "Desc km" : "Km";
            ViewBag.SortingPrix = sortOrder == "Prix" ? "Desc prix" : "Prix";

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }
            ViewBag.CurrentFilter = search;

            var cars = from car in db.vehicule select car;

            if (!String.IsNullOrEmpty(search))
            {
                cars = cars.Where(x =>
                                            x.marque.ToUpper().Contains(search.ToUpper()) ||
                                            x.modele.ToString().ToUpper().Contains(search.ToUpper()) ||
                                            x.annee.ToString().ToUpper().Contains(search.ToUpper()) ||
                                            x.kilometrage.ToString().ToUpper().Contains(search.ToUpper()) ||
                                            x.prix.ToString().ToUpper().Contains(search.ToUpper()));
            }

            switch (sortOrder)
            {
                case "Desc parkingId":
                    cars = cars.OrderByDescending(x => x.vehiculeId);
                    break;

                case "Desc Marque":
                    cars = cars.OrderByDescending(x => x.marque);
                    break;
                case "Marque":
                    cars = cars.OrderBy(x => x.marque);
                    break;

                case "Desc modele":
                    cars = cars.OrderByDescending(x => x.modele);
                    break;
                case "Modele":
                    cars = cars.OrderBy(x => x.modele);
                    break;

                case "Desc annee":
                    cars = cars.OrderByDescending(x => x.annee);
                    break;
                case "Annee":
                    cars = cars.OrderBy(x => x.annee);
                    break;

                case "Desc km":
                    cars = cars.OrderByDescending(x => x.kilometrage);
                    break;
                case "Km":
                    cars = cars.OrderBy(x => x.kilometrage);
                    break;

                case "Desc prix":
                    cars = cars.OrderByDescending(x => x.prix);
                    break;
                case "Prix":
                    cars = cars.OrderBy(x => x.prix);
                    break;

                default:
                    cars = cars.OrderBy(x => x.vehiculeId);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(cars.ToPagedList(pageNumber, pageSize));
        }

        // GET: vehicules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicule vehicule = db.vehicule.Find(id);
            if (vehicule == null)
            {
                return HttpNotFound();
            }
            return View(vehicule);
        }

        // GET: vehicules/Create
        public ActionResult Create()
        {
            ViewBag.maintenanceFK = new SelectList(db.maintenance, "PK_maintenanceId", "PK_maintenanceId");
            ViewBag.parkingFK = new SelectList(db.parking, "parkingId", "name");
            Vehicule vehicule = new Vehicule();
            return View(vehicule);
        }

        // POST: vehicules/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "vehiculeId,marque,modele,annee,kilometrage,photo,prix,maintenanceFK,parkingFK")] Vehicule vehicule)
        {
            if (ModelState.IsValid)
            {
                db.vehicule.Add(vehicule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.maintenanceFK = new SelectList(db.maintenance, "PK_maintenanceId", "PK_maintenanceId", vehicule.maintenanceFK);
            ViewBag.parkingFK = new SelectList(db.parking, "parkingId", "name", vehicule.parkingFK);
            return View(vehicule);
        }

        // GET: vehicules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicule vehicule = db.vehicule.Find(id);
            if (vehicule == null)
            {
                return HttpNotFound();
            }
            ViewBag.maintenanceFK = new SelectList(db.maintenance, "PK_maintenanceId", "PK_maintenanceId", vehicule.maintenanceFK);
            ViewBag.parkingFK = new SelectList(db.parking, "parkingId", "name", vehicule.parkingFK);
            return View(vehicule);
        }

        // POST: vehicules/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "vehiculeId,marque,modele,annee,kilometrage,photo,prix,maintenanceFK,parkingFK")] Vehicule vehicule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.maintenanceFK = new SelectList(db.maintenance, "PK_maintenanceId", "PK_maintenanceId", vehicule.maintenanceFK);
            ViewBag.parkingFK = new SelectList(db.parking, "parkingId", "name", vehicule.parkingFK);
            return View(vehicule);
        }

        // GET: vehicules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicule vehicule = db.vehicule.Find(id);
            if (vehicule == null)
            {
                return HttpNotFound();
            }
            return View(vehicule);
        }

        // POST: vehicules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicule vehicule = db.vehicule.Find(id);
            db.vehicule.Remove(vehicule);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
