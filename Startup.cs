using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SNKRS.Startup))]
namespace SNKRS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            InitAdmin();
        }

        public void InitAdmin()
        {

        }
    }
}
