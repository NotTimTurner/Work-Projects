using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WMSScanner.Startup))]
namespace WMSScanner
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
