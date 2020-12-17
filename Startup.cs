using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectLab04.Startup))]
namespace ProjectLab04
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
