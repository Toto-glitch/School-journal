using System;
using System.Windows;
using System.Windows.Controls;
using SchoolJournal.View.Pages;

namespace SchoolJournal
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new DashboardPage());
            CurrentDate.Text = DateTime.Now.ToString("dd.MM.yyyy");
        }

        private void NavigateToPage(object sender, RoutedEventArgs e, Page page, string title)
        {
            ContentFrame.Navigate(page);
            PageTitle.Text = title;
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e) =>
            NavigateToPage(sender, e, new DashboardPage(), "Панель управления");

        private void Journal_Click(object sender, RoutedEventArgs e) =>
            NavigateToPage(sender, e, new JournalPage(), "Классный журнал");

        private void Diary_Click(object sender, RoutedEventArgs e) =>
            NavigateToPage(sender, e, new DiaryPage(), "Дневник ученика");

        private void Students_Click(object sender, RoutedEventArgs e) =>
            NavigateToPage(sender, e, new StudentsPage(), "Список учеников");

        private void Teachers_Click(object sender, RoutedEventArgs e) =>
            NavigateToPage(sender, e, new TeachersPage(), "Список учителей");

        private void Subjects_Click(object sender, RoutedEventArgs e) =>
            NavigateToPage(sender, e, new SubjectsPage(), "Предметы");

        private void Parents_Click(object sender, RoutedEventArgs e) =>
            NavigateToPage(sender, e, new ParentsPage(), "Список родителей");

        private void Marks_Click(object sender, RoutedEventArgs e) =>
            NavigateToPage(sender, e, new MarksPage(), "Управление оценками");

        private void Reports_Click(object sender, RoutedEventArgs e) =>
            NavigateToPage(sender, e, new ReportsPage(), "Отчеты и аналитика");

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Выйти из системы?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }
    }
}