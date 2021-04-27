using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace diplom.Pages
{
    /// <summary>
    /// Логика взаимодействия для Results.xaml
    /// </summary>
    public partial class PageResults : Page
    {
        public PageResults()
        {
            InitializeComponent();
        }
        public void LoadResults()
        {
            stackPanel.Children.Clear();
            List<ResultPreview> resultsList = new List<ResultPreview>();

            SqlConnection connection = new SqlConnection("Data Source=DESKTOP-JOHEOHB\\SQLEXPRESS;Initial Catalog=dpl;User ID=sa;Password=12345");
            connection.Open();
            string command = "select Результаты.Идентификатор as Идентификатор, Тесты.Название as Тест, Шкалы.Название as Шкала, Пользователи.Имя as Имя, Пользователи.Фамилия as Фамилия, Пользователи.Логин as Логин, Значение, Максимум " +
                "from Результаты " +
                "join Пользователи on Пользователи.Идентификатор = Результаты.Идентификатор_пользователя " +
                "join Тесты on Тесты.Идентификатор = Результаты.Идентификатор_теста " +
                "join Шкалы on Тесты.Идентификатор_шкалы = Шкалы.Идентификатор";
            if (MainWindow.Role == "user")
            {
                command += " where Логин = '" + MainWindow.Login+"'";
            }
            SqlDataReader reader = new SqlCommand(command, connection).ExecuteReader();
            while (reader.Read())
            {
                ResultPreview preview = new ResultPreview(reader.GetInt32(reader.GetOrdinal("Идентификатор")), reader.GetString(reader.GetOrdinal("Тест")), reader.GetString(reader.GetOrdinal("Имя")), reader.GetString(reader.GetOrdinal("Фамилия")), reader.GetString(reader.GetOrdinal("Логин")), reader.GetInt32(reader.GetOrdinal("Значение")), reader.GetInt32(reader.GetOrdinal("Максимум")), reader.GetString(reader.GetOrdinal("Шкала")));
                resultsList.Add(preview);
            }
            reader.Close();

            if (resultsList.Count < 1)
            {
                Label label = new Label();
                label.FontSize = 24;
                label.Content = "Пока нет результатов для указанного пользователя";
                label.HorizontalAlignment = HorizontalAlignment.Center;
                stackPanel.Children.Add(label);
                return;
            }
            foreach (ResultPreview result in resultsList)
            {
                Border border = new Border();
                border.BorderThickness = new Thickness(1);
                border.BorderBrush = Brushes.Black;
                border.Margin = new Thickness(10);

                TextBlock resultText = new TextBlock();
                resultText.Text = result.Test+"\n"+ result.Surname + " " + result.Name + " (" + result.Login + ")\nБаллы: "+ result.Value + " из " + result.Maximum + " (" + result.Scale + ")\n"+ result.Test;
                resultText.Height = 65;
                resultText.Margin = new Thickness(10);

                border.Child = resultText;
                stackPanel.Children.Add(border);
            }
        }
    }
}
