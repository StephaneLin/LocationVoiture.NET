using Location_AAE_ASP_NET.Infrastructure;
using Location_AAE_ASP_NET.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Location_AAE_ASP_NET.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private Model1 db = new Model1();

        public AccountController()
        {
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            string email = collection["email"];
            string password = collection["password"];

            string query = "SELECT * FROM utilisateur WHERE email = @p0";
            Utilisateur user = await db.utilisateur.SqlQuery(query, email).SingleOrDefaultAsync();

            if (null == user || user.password != password)
            {
                return View();
            }

            Session["user"] = user;
            Session["userEmail"] = user.email;
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            Utilisateur newUser = new Utilisateur();
            newUser.connexionDate = DateTime.UtcNow.ToString();
            newUser.userId = 0;
            newUser.entity = "user";
            newUser.companyPhone = "0";
            newUser.companyName = "0";
            newUser.siret = "0";
            ViewBag.accessRightsFK = new SelectList(db.droit_acces, "droitAccesId", "role");
            return View(newUser);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "userId,entity,connexionDate,companyName,companyPhone,siret,userLastName,userFirstName,userPhone,userYearOld,email,password,licence,accessRightsFK")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                db.utilisateur.Add(utilisateur);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.accessRightsFK = new SelectList(db.droit_acces, "droitAccesId", "role", utilisateur.accessRightsFK);
            return View("Register");
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [AuthenticationFilter]
        public ActionResult LogOff()
        {
            Session["user"] = null;
            Session["userEmail"] = null;

            return RedirectToAction("Index", "Home");
        }


        // GET: /Account/Manage
        [AuthenticationFilter]
        [AllowAnonymous]
        public ActionResult Manage()
        {
            Utilisateur user = (Utilisateur)Session["user"];
            Utilisateur utilisateur = db.utilisateur.Find(user.userId);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            ViewBag.accessRightsFK = new SelectList(db.droit_acces, "droitAccesId", "role", utilisateur.accessRightsFK);
            return View(utilisateur);
        }

        // POST: /Account/Manage
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [AuthenticationFilter]
        public ActionResult Manage([Bind(Include = "userId,entity,connexionDate,companyName,companyPhone,siret,userLastName,userFirstName,userPhone,userYearOld,email,password,licence,accessRightsFK")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                db.Entry(utilisateur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Home", "Index");
            }
            ViewBag.accessRightsFK = new SelectList(db.droit_acces, "droitAccesId", "role", utilisateur.accessRightsFK);
            return View(utilisateur);
        }

        #region Applications auxiliaires
        // Utilisé(e) pour la protection XSRF lors de l'ajout de connexions externes
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}