using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Events4All.Web.Startup))]
namespace Events4All.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
