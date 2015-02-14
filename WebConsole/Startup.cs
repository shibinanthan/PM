using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebConsole.Startup))]
namespace WebConsole
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
