using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using diplom.Pages;

namespace diplom
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CurrentPageAuth         = new PageAuth();
            CurrentPageMenu         = new PageMenu();
            CurrentPageResults      = new PageResults();
            CurrentPagePrescription = new PagePrescription();
            CurrentPageRedactor     = new PageRedactor();
            CurrentPageTest         = new PageTest();
            CurrentPageTestsList    = new PageTestsList();
            CurrentPageRes          = new PageRes();

            CurrentBackButton = BackButton;
            CurrentLabelName  = LabelName;
            CurrentFrame      = MainFrame;

            MainFrame.Navigate(CurrentPageAuth);
        }

        public static PageAuth         CurrentPageAuth;
        public static PageMenu         CurrentPageMenu;
        public static PageResults      CurrentPageResults;
        public static PagePrescription CurrentPagePrescription;
        public static PageRedactor     CurrentPageRedactor;
        public static PageTest         CurrentPageTest;
        public static PageTestsList    CurrentPageTestsList;
        public static PageRes          CurrentPageRes;
        public static Button           CurrentBackButton;
        public static Label            CurrentLabelName;
        public static Frame            CurrentFrame;
        public static string           Role;
        public static string           Login;
        public static int              UserId;
        static string BackButtonMode = "Exit";
        public static void EnterMenu()
        {
            BackButtonMode = "ToLogin";
            CurrentBackButton.Content = "Выйти";
            CurrentPageMenu.LoadButtons();
            CurrentFrame.Navigate(CurrentPageMenu);
        }
        public static void GoToLogin()
        {
            BackButtonMode = "Exit";
            CurrentPageAuth.ClearCredentials();
            CurrentFrame.Navigate(CurrentPageAuth);
        }
        public static void GoToTestsList()
        {
            BackButtonMode = "Menu";
            CurrentBackButton.Content = "В меню";
            CurrentPageTestsList.LoadTestsList();
            CurrentFrame.Navigate(CurrentPageTestsList);
        }
        public static void GoToResultsList()
        {
            BackButtonMode = "Menu";
            CurrentBackButton.Content = "В меню";
            CurrentPageResults.LoadResults();
            CurrentFrame.Navigate(CurrentPageResults);
        }
        public static void GoToPrescriptions()
        {
            BackButtonMode = "Menu";
            CurrentBackButton.Content = "В меню";
            CurrentFrame.Navigate(CurrentPagePrescription);
        }
        public static void StartTest(int id)
        {
            BackButtonMode = "Menu";
            CurrentBackButton.Content = "В меню";
            CurrentPageTest.LoadTest(id);
            CurrentFrame.Navigate(CurrentPageTest);
        }
        public static void ShowTestResult(int result)
        {
            BackButtonMode = "Menu";
            CurrentBackButton.Content = "В меню";
            CurrentPageRes.ShowResult(result);
            CurrentFrame.Navigate(CurrentPageRes);
        }
        public static void CreateTest()
        {
            BackButtonMode = "Menu";
            CurrentBackButton.Content = "В меню";
            CurrentPageRedactor.StartNew();
            CurrentFrame.Navigate(CurrentPageRedactor);
        }

        public static void SetLabelName(string text)
        {
            CurrentLabelName.Content = text;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            switch (BackButtonMode)
            {
                case "Exit":    Application.Current.Shutdown(); break;
                case "Menu":    EnterMenu();                    break;
                case "ToLogin": GoToLogin();                    break;
                default: throw new Exception("Unexpected BackButton mode");
            }
        }
    }
}
