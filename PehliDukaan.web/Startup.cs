using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PehliDukaan.web.Startup))]
namespace PehliDukaan.web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
