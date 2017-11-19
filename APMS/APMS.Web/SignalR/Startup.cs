using Microsoft.Owin;
using Owin;

namespace APMS.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}