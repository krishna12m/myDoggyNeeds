using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyDoggyNeeds.Startup))]
namespace MyDoggyNeeds
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
