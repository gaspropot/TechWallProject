using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TechWall.Startup))]
namespace TechWall
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
            
        }
    }
}
