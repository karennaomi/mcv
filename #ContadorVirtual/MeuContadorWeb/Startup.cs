using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MeuContadorWeb.Startup))]
namespace MeuContadorWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
