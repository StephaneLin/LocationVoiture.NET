using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Location_AAE_ASP_NET.Startup))]
namespace Location_AAE_ASP_NET
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
