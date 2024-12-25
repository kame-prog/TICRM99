using Owin;
using System.Web.Services.Description;

namespace TICRM.UI.ASPNetMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
          
        }
        
    }
}
