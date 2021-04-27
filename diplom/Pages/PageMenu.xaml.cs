using System.Windows;
using System.Windows.Controls;

namespace diplom.Pages
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class PageMenu : Page
    {
        public PageMenu()
        {
            InitializeComponent();
        }

        public void LoadButtons()
        {
            switch (MainWindow.Role)
            {
                case "admin":
                    Button1.IsEnabled = true;
                    Button2.IsEnabled = true;
                    Button3.IsEnabled = true;
                    Button4.IsEnabled = true;
                    break;
                case "user":
                    Button1.IsEnabled = true;
                    Button2.IsEnabled = true;
                    Button3.IsEnabled = false;
                    Button4.IsEnabled = false;
                    break;
                default:
                    Button1.IsEnabled = false;
                    Button2.IsEnabled = false;
                    Button3.IsEnabled = false;
                    Button4.IsEnabled = false;
                    break;
            }
        }

        private void GoToTestsList(object sender, RoutedEventArgs e)
        {
            MainWindow.GoToTestsList();
        }

        private void CreateTest(object sender, RoutedEventArgs e)
        {
            MainWindow.CreateTest();
        }

        private void GoToResults(object sender, RoutedEventArgs e)
        {
            MainWindow.GoToResultsList();
        }

        private void GoToPrescriptions(object sender, RoutedEventArgs e)
        {
            MainWindow.GoToPrescriptions();
        }
    }
}
