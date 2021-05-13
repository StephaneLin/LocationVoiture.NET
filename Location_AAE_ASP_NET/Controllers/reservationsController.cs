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
    public class ReservationsController : Controller
    {
        private Model1 db = new Model1();

        // GET: reservations
        public ActionResult Index(string sortOrder, string search, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortingId = String.IsNullOrEmpty(sortOrder) ? "Desc reservationId" : "";
            ViewBag.SortingDate = sortOrder == "Date" ? "Desc date" : "Date";
            ViewBag.SortingDebutDate = sortOrder == "DebutDate" ? "Desc debutDate" : "DebutDate";
            ViewBag.SortingFinDate = sortOrder == "FinDate" ? "Desc finDate" : "FinDate";
            ViewBag.SortingStatus = sortOrder == "Status" ? "Desc status" : "Status";

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }
            ViewBag.CurrentFilter = search;

            var reservations = from reservation in db.reservation select reservation;

            if (!String.IsNullOrEmpty(search))
            {
                reservations = reservations.Where(x =>
                                            x.utilisateur.email.ToUpper().Contains(search.ToUpper()) ||
                                            x.vehicule.modele.ToString().ToUpper().Contains(search.ToUpper()) ||
                                            x.debutDate.ToString().ToUpper().Contains(search.ToUpper()) ||
                                            x.finDate.ToString().ToUpper().Contains(search.ToUpper()) ||
                                            x.reservationDate.ToString().ToUpper().Contains(search.ToUpper())
                                            );
            }

            switch (sortOrder)
            {
                case "Desc reservationId":
                    reservations = reservations.OrderByDescending(x => x.reservationId);
                    break;

                case "Desc date":
                    reservations = reservations.OrderByDescending(x => x.reservationDate);
                    break;
                case "Date":
                    reservations = reservations.OrderBy(x => x.reservationDate);
                    break;

                case "Desc debutDate":
                    reservations = reservations.OrderByDescending(x => x.debutDate);
                    break;
                case "DebutDate":
                    reservations = reservations.OrderBy(x => x.debutDate);
                    break;

                case "Desc finDate":
                    reservations = reservations.OrderByDescending(x => x.finDate);
                    break;
                case "FinDate":
                    reservations = reservations.OrderBy(x => x.finDate);
                    break;

                case "Desc status":
                    reservations = reservations.OrderByDescending(x => x.status);
                    break;
                case "Status":
                    reservations = reservations.OrderBy(x => x.status);
                    break;

                default:
                    reservations = reservations.OrderBy(x => x.reservationId);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(reservations.ToPagedList(pageNumber, pageSize));
        }

        // GET: reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.reservation.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: reservations/Create
        public ActionResult Create()
        {
            ViewBag.utilisateurFK = new SelectList(db.utilisateur, "userId", "email");
            ViewBag.vehiculeFK = new SelectList(db.vehicule, "vehiculeId", "vehiculeId");
            Reservation newReservation = new Reservation();
            newReservation.reservationDate = DateTime.UtcNow;
            return View(newReservation);
        }

        // POST: reservations/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "reservationId,debutDate,finDate,status,reservationDate,vehiculeFK,utilisateurFK")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.reservation.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.utilisateurFK = new SelectList(db.utilisateur, "userId", "email", reservation.utilisateurFK);
            ViewBag.vehiculeFK = new SelectList(db.vehicule, "vehiculeId", "vehiculeId", reservation.vehiculeFK);
            return View(reservation);
        }

        // GET: reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.reservation.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.utilisateurFK = new SelectList(db.utilisateur, "userId", "email", reservation.utilisateurFK);
            ViewBag.vehiculeFK = new SelectList(db.vehicule, "vehiculeId", "vehiculeId", reservation.vehiculeFK);
            return View(reservation);
        }

        // POST: reservations/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "reservationId,debutDate,finDate,status,reservationDate,vehiculeFK,utilisateurFK")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.utilisateurFK = new SelectList(db.utilisateur, "userId", "email", reservation.utilisateurFK);
            ViewBag.vehiculeFK = new SelectList(db.vehicule, "vehiculeId", "vehiculeId", reservation.vehiculeFK);
            return View(reservation);
        }

        // GET: reservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.reservation.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.reservation.Find(id);
            db.reservation.Remove(reservation);
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
