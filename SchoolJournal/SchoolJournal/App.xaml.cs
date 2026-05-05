using SchoolJournal.Model;
using System.Windows;

namespace SchoolJournal
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var loginWindow = new LoginWindow();
            loginWindow.Show();

            using (var context = new ApplicationContext())
            {
                context.Database.Initialize(false);
            }
        }
    }
}