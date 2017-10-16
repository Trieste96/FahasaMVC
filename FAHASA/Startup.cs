using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FAHASA.Startup))]
namespace FAHASA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
