using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EvaluationApp.Startup))]
namespace EvaluationApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
