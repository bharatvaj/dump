using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CommunicationsHost.Startup))]
namespace CommunicationsHost
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
