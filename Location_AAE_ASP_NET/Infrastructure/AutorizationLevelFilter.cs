using Location_AAE_ASP_NET.Models;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Location_AAE_ASP_NET.Infrastructure
{
    public class AutorizationLevelFilter : AuthorizeAttribute
    {
        private readonly int allowedRole;
        public AutorizationLevelFilter(int role)
        {
            this.allowedRole = role;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            Utilisateur user = (Utilisateur)httpContext.Session["user"];
            if (null != user)
            {
                if (user.droit_acces.droitAccesId >= allowedRole)
                {
                    authorize = true;
                }
            }

            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Home" },
                    { "action", "UnAuthorized" }
               });
        }
    }
}