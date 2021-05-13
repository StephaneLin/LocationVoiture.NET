using Location_AAE_ASP_NET.Infrastructure;
using Location_AAE_ASP_NET.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Location_AAE_ASP_NET.Controllers
{
    [AuthenticationFilter]
    [AutorizationLevelFilter(3)]
    public class UtilisateursController : Controller
    {
        private Model1 db = new Model1();

        // GET: utilisateurs
        public async System.Threading.Tasks.Task<ActionResult> Index(string sortOrder, string search, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortingNumber = String.IsNullOrEmpty(sortOrder) ? "userId" : "";
            ViewBag.SortingDate = sortOrder == "ConnexionDate" ? "Desc connexionDate" : "ConnexionDate";
            ViewBag.SortingCompanyName = sortOrder == "CompanyName" ? "Desc companyName" : "CompanyName";
            ViewBag.SortingCompanyPhone = sortOrder == "CompanyPhone" ? "Desc companyPhone" : "CompanyPhone";
            ViewBag.SortingSiret = sortOrder == "Siret" ? "Desc siret" : "Siret";
            ViewBag.SortingLastName = sortOrder == "LastName" ? "Desc lastName" : "LastName";
            ViewBag.SortingFirstName = sortOrder == "FirstName" ? "Desc firstName" : "FirstName";
            ViewBag.SortingUserPhone = sortOrder == "UserPhone" ? "Desc userPhone" : "UserPhone";
            ViewBag.SortingYearOld = sortOrder == "Age" ? "Desc age" : "Age";
            ViewBag.SortingEmail = sortOrder == "Email" ? "Desc email" : "Email";

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }
            ViewBag.CurrentFilter = search;

            var utilisateur = from user in db.utilisateur select user;
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(search))
            {
                if (int.TryParse(search, out int i))
                {
                    string query = "SELECT * FROM utilisateur WHERE @p0 IN (userId, accessRightsFK)";
                    List<Utilisateur> users = await db.utilisateur.SqlQuery(query, i).ToListAsync();
                    return View(users.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    string query = "SELECT * FROM utilisateur WHERE @p0 IN (entity, connexionDate, companyName, companyPhone, siret, userLastName, userFirstName, userPhone, userYearOld, email, licence)";
                    List<Utilisateur> users = await db.utilisateur.SqlQuery(query, search).ToListAsync();
                    return View(users.ToPagedList(pageNumber, pageSize));
                }
            }

            switch (sortOrder)
            {
                case "Desc userId":
                    utilisateur = utilisateur.OrderByDescending(x => x.userId);
                    break;

                case "Desc connexionDate":
                    utilisateur = utilisateur.OrderByDescending(x => x.connexionDate);
                    break;
                case "ConnexionDate":
                    utilisateur = utilisateur.OrderBy(x => x.connexionDate);
                    break;

                case "Desc companyName":
                    utilisateur = utilisateur.OrderByDescending(x => x.companyName);
                    break;
                case "CompanyName":
                    utilisateur = utilisateur.OrderBy(x => x.companyName);
                    break;

                case "Desc companyPhone":
                    utilisateur = utilisateur.OrderByDescending(x => x.companyPhone);
                    break;
                case "CompanyPhone":
                    utilisateur = utilisateur.OrderBy(x => x.companyPhone);
                    break;

                case "Desc siret":
                    utilisateur = utilisateur.OrderByDescending(x => x.siret);
                    break;
                case "Siret":
                    utilisateur = utilisateur.OrderBy(x => x.siret);
                    break;

                case "Desc lastName":
                    utilisateur = utilisateur.OrderByDescending(x => x.userLastName);
                    break;
                case "LastName":
                    utilisateur = utilisateur.OrderBy(x => x.userLastName);
                    break;

                case "Desc firstName":
                    utilisateur = utilisateur.OrderByDescending(x => x.userFirstName);
                    break;
                case "FirstName":
                    utilisateur = utilisateur.OrderBy(x => x.userFirstName);
                    break;

                case "Desc userPhone":
                    utilisateur = utilisateur.OrderByDescending(x => x.userPhone);
                    break;
                case "UserPhone":
                    utilisateur = utilisateur.OrderBy(x => x.userPhone);
                    break;

                case "Desc age":
                    utilisateur = utilisateur.OrderByDescending(x => x.userYearOld);
                    break;
                case "Age":
                    utilisateur = utilisateur.OrderBy(x => x.userYearOld);
                    break;

                case "Desc email":
                    utilisateur = utilisateur.OrderByDescending(x => x.email);
                    break;
                case "Email":
                    utilisateur = utilisateur.OrderBy(x => x.email);
                    break;

                default:
                    utilisateur = utilisateur.OrderBy(x => x.userId);
                    break;
            }
            return View(utilisateur.ToPagedList(pageNumber, pageSize));
        }

        // GET: utilisateurs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.utilisateur.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // GET: utilisateurs/Create
        public ActionResult Create()
        {
            Utilisateur newUser = new Utilisateur();
            newUser.connexionDate = DateTime.UtcNow.ToString();
            newUser.userFirstName = "0";
            newUser.companyName = "user";
            newUser.userLastName = "0";
            newUser.userYearOld = "0";
            newUser.userPhone = "0";
            newUser.companyName = "0";
            newUser.companyPhone = "0";
            newUser.siret = "0";
            ViewBag.accessRightsFK = new SelectList(db.droit_acces, "droitAccesId", "role");
            return View(newUser);
        }

        // POST: utilisateurs/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userId,entity,connexionDate,companyName,companyPhone,siret,userLastName,userFirstName,userPhone,userYearOld,email,password,licence,accessRightsFK")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                db.utilisateur.Add(utilisateur);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.accessRightsFK = new SelectList(db.droit_acces, "droitAccesId", "role", utilisateur.accessRightsFK);
            return View(utilisateur);
        }

        // GET: utilisateurs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.utilisateur.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            ViewBag.accessRightsFK = new SelectList(db.droit_acces, "droitAccesId", "role", utilisateur.accessRightsFK);
            return View(utilisateur);
        }

        // POST: utilisateurs/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userId,entity,connexionDate,companyName,companyPhone,siret,userLastName,userFirstName,userPhone,userYearOld,email,password,licence,accessRightsFK")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                db.Entry(utilisateur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.accessRightsFK = new SelectList(db.droit_acces, "droitAccesId", "role", utilisateur.accessRightsFK);
            return View(utilisateur);
        }

        // GET: utilisateurs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.utilisateur.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // POST: utilisateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Utilisateur utilisateur = db.utilisateur.Find(id);
            db.utilisateur.Remove(utilisateur);
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
