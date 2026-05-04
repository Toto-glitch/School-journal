using System.Windows;

namespace SchoolDiary
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Установка инициализатора БД
            System.Data.Entity.Database.SetInitializer(new Data.DbInitializer());

            // Принудительное создание БД при первом запуске
            using (var context = new Models.SchoolDbContext())
            {
                context.Database.Initialize(force: true);
            }
        }
    }
}
