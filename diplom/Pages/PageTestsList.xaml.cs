using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace diplom.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageTestsList.xaml
    /// </summary>
    public partial class PageTestsList : Page
    {
        public PageTestsList()
        {
            InitializeComponent();
        }

        List<TestPreview> testsList = new List<TestPreview>();

        public void LoadTestsList()
        {
            stackPanel.Children.Clear();
            testsList = new List<TestPreview>();

            SqlConnection connection = new SqlConnection("Data Source=DESKTOP-JOHEOHB\\SQLEXPRESS;Initial Catalog=dpl;User ID=sa;Password=12345");
            connection.Open();
            SqlDataReader reader = new SqlCommand("select * from Тесты", connection).ExecuteReader();
            while (reader.Read())
            {
                TestPreview preview = new TestPreview(reader.GetInt32(reader.GetOrdinal("Идентификатор")), reader.GetString(reader.GetOrdinal("Название")));
                testsList.Add(preview);
            }
            reader.Close();

            if (testsList.Count < 1)
            {
                Label label = new Label();
                label.FontSize = 24;
                label.Content = "Не удалось загрузить тесты";
                stackPanel.Children.Add(label);
                return;
            }
            foreach (TestPreview test in testsList)
            {
                Button testButton = new Button();
                testButton.Content = test.Name;
                testButton.Height = 40;
                testButton.Margin = new Thickness(10);
                testButton.AddHandler(Button.ClickEvent, new RoutedEventHandler(StartTest));
                stackPanel.Children.Add(testButton);
            }
        }
        void StartTest(object sender, RoutedEventArgs e)
        {
            int id = -1;
            Button button = sender as Button;
            string testName = button.Content.ToString();
            foreach (TestPreview test in testsList)
            {
                if (test.Name == testName)
                {
                    id = test.Id;
                }
            }
            if (id == -1)
            {
                throw new Exception("Тест не найден");
            }
            MainWindow.StartTest(id);
        }

    }
}
