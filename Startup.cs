using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Co_nnecto.Startup))]
namespace Co_nnecto
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
