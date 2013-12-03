using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Telestrations.Startup))]
namespace Telestrations
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
