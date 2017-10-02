using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Group_Ledger.Startup))]
namespace Group_Ledger
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
