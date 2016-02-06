using System.Net;
using System.Windows;

namespace Yatse2Setup
{
    public partial class App
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            ServicePointManager.DefaultConnectionLimit = 10;
            base.OnStartup(e);
        }
    }
}
