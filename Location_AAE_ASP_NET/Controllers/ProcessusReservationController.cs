using Location_AAE_ASP_NET.Infrastructure;
using Location_AAE_ASP_NET.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Location_AAE_ASP_NET.Controllers
{
    public class ProcessusReservationController : Controller
    {
        private Model1 db = new Model1();

        [AuthenticationFilter]
        public ActionResult BookedVehicles()
        {
            Utilisateur user = (Utilisateur)Session["user"];
            var reservation = from res in db.reservation where user.userId == res.utilisateurFK select res;
            return View(reservation.ToList());
        }

        // GET
        public ActionResult DateAndPlaceChoice()
        {
            ViewBag.name = new SelectList(db.parking, "parkingId", "name");
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DateAndPlaceChoice(FormCollection collection)
        {
            int parkName = Int32.Parse(collection["name"]);
            var date = collection["StartMonth"];
            String startDate = collection["StartYear"] + "." + collection["StartMonth"] + "." + collection["StartDay"];
            String endDate = collection["EndYear"] + "." + collection["EndMonth"] + "." + collection["EndDay"];
            DateTime dtStart = DateTime.ParseExact(startDate, "yyyy.mm.dd", null);
            DateTime dtEnd = DateTime.ParseExact(endDate, "yyyy.mm.dd", null);
            SqlDateTime start = SqlDateTime.Parse(dtStart.ToString("yyyy/mm/dd"));
            SqlDateTime end = SqlDateTime.Parse(dtEnd.ToString("yyyy/mm/dd"));

            if (ModelState.IsValid)
            {
                string query = "SELECT vehicule.* FROM vehicule " +
                    "LEFT JOIN reservation ON vehicule.vehiculeId = reservation.vehiculeFK " +
                    "WHERE vehicule.parkingFK = @p0 " +
                    "AND ( reservation.debutDate >= @p1 OR reservation.finDate <= @p2) " +
                    "EXCEPT SELECT vehicule.* " +
                    "FROM vehicule " +
                    "LEFT JOIN reservation ON vehicule.vehiculeId = reservation.vehiculeFK " +
                    "WHERE vehicule.parkingFK = @p0 " +
                    "AND NOT(reservation.debutDate >= @p1 OR reservation.finDate <= @p2)";
                IList<Vehicule> carList = await db.vehicule.SqlQuery(query, parkName, end, start).ToListAsync();
                TempData["voituresRequete"] = carList;
                Session["startDate"] = dtStart;
                Session["endDate"] = dtEnd;

                return RedirectToAction("BookableVehicles");
            }

            return View();
        }

        [AuthenticationFilter]
        public ActionResult Book(int? id)
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

        public ActionResult BookableVehicles()
        {
            IList<Vehicule> cars = (IList<Vehicule>)TempData["voituresRequete"];
            return View();
        }

        // GET: ProcessusReservation
        public ActionResult Index()
        {
            return View();
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
            return RedirectToAction("BookedVehicles");
        }

        // POST: reservation/BookConfirmed
        [AuthenticationFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Book(FormCollection collection)
        {
            Utilisateur user = (Utilisateur)Session["user"];
            Vehicule car = db.vehicule.Find(int.Parse(collection["vehiculeFK"]));
            String[] startDate = collection["startDate"].Split(' ');
            String[] endDate = collection["endDate"].Split(' ');

            DateTime dtStart = DateTime.ParseExact(startDate[0], "dd/mm/yyyy", null);
            DateTime dtEnd = DateTime.ParseExact(endDate[0], "dd/mm/yyyy", null);

            Reservation reservation = new Reservation
            {
                reservationId = 0,
                vehiculeFK = car.vehiculeId,
                debutDate = dtStart,
                finDate = dtEnd,
                reservationDate = DateTime.UtcNow,
                status = 0,
                utilisateurFK = user.userId
            };
            db.reservation.Add(reservation);
            db.SaveChanges();
            return RedirectToAction("BookedVehicles");
        }
    }
}