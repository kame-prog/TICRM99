using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TICRM.Startup))]
namespace TICRM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
