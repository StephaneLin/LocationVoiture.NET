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
    [AutorizationLevelFilter(3)]
    public class ParkingsController : Controller
    {
        private Model1 db = new Model1();

        // GET: parkings
        public ActionResult Index(string sortOrder, string search, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortingId = String.IsNullOrEmpty(sortOrder) ? "Desc name" : "";
            ViewBag.SortingAddress = sortOrder == "Address" ? "Desc address" : "Address";
            ViewBag.SortingNumber = sortOrder == "Number" ? "Desc number" : "Number";
            ViewBag.SortingPhone = sortOrder == "Phone" ? "Desc phone" : "Phone";

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }

            ViewBag.CurrentFilter = search;

            var parkings = from park in db.parking select park;

            if (!String.IsNullOrEmpty(search))
            {
                parkings = parkings.Where(x =>
                                            x.name.ToUpper().Contains(search.ToUpper()) ||
                                            x.address.ToUpper().Contains(search.ToUpper()) ||
                                            x.nbrCar.ToString().ToUpper().Contains(search.ToUpper()) ||
                                            x.phoneNumber.ToString().ToUpper().Contains(search.ToUpper())
                                            );
            }

            switch (sortOrder)
            {
                case "Desc name":
                    parkings = parkings.OrderByDescending(x => x.name);
                    break;

                case "Desc address":
                    parkings = parkings.OrderByDescending(x => x.address);
                    break;
                case "Address":
                    parkings = parkings.OrderBy(x => x.address);
                    break;

                case "Desc nbrCar":
                    parkings = parkings.OrderByDescending(x => x.nbrCar);
                    break;
                case "NbrCar":
                    parkings = parkings.OrderBy(x => x.nbrCar);
                    break;

                case "Desc phone":
                    parkings = parkings.OrderByDescending(x => x.phoneNumber);
                    break;
                case "Phone":
                    parkings = parkings.OrderBy(x => x.phoneNumber);
                    break;

                default:
                    parkings = parkings.OrderBy(x => x.name);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(parkings.ToPagedList(pageNumber, pageSize));

        }

        // GET: parkings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parking parking = db.parking.Find(id);
            if (parking == null)
            {
                return HttpNotFound();
            }
            return View(parking);
        }

        // GET: parkings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: parkings/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "parkingId,name,address,nbrCar,phoneNumber")] Parking parking)
        {
            if (ModelState.IsValid)
            {
                db.parking.Add(parking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parking);
        }

        // GET: parkings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parking parking = db.parking.Find(id);
            if (parking == null)
            {
                return HttpNotFound();
            }
            return View(parking);
        }

        // POST: parkings/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "parkingId,name,address,nbrCar,phoneNumber")] Parking parking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parking);
        }

        // GET: parkings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parking parking = db.parking.Find(id);
            if (parking == null)
            {
                return HttpNotFound();
            }
            return View(parking);
        }

        // POST: parkings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Parking parking = db.parking.Find(id);
            db.parking.Remove(parking);
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
