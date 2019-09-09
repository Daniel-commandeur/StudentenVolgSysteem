using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StudentenVolgSysteem.Startup))]
namespace StudentenVolgSysteem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
