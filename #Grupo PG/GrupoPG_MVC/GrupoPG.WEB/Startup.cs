using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GrupoPG.WEB.Startup))]
namespace GrupoPG.WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
