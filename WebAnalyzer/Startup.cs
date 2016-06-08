using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAnalyzer.Startup))]
namespace WebAnalyzer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
