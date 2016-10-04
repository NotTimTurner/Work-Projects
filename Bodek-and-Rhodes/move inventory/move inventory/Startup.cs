using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(move_inventory.Startup))]
namespace move_inventory
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
