using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WEBCAM.Startup))]
namespace WEBCAM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
