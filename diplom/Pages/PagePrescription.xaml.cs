using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace diplom.Pages
{
    /// <summary>
    /// Логика взаимодействия для PagePrescription.xaml
    /// </summary>
    public partial class PagePrescription : Page
    {
        public PagePrescription()
        {
            InitializeComponent();

            users = new List<string>();

            SqlConnection connection = new SqlConnection("Data Source=DESKTOP-JOHEOHB\\SQLEXPRESS;Initial Catalog=dpl;User ID=sa;Password=12345");
            connection.Open();
            string command = "select Имя, Фамилия, Отчество, Логин from Пользователи";
            SqlDataReader reader = new SqlCommand(command, connection).ExecuteReader();
            while (reader.Read())
            {
                users.Add(reader.GetString(reader.GetOrdinal("Фамилия")) + " " + reader.GetString(reader.GetOrdinal("Имя"))[0] + "." + reader.GetString(reader.GetOrdinal("Отчество"))[0] + " (" + reader.GetString(reader.GetOrdinal("Логин")) + ")");
            }
            UserComboBox.ItemsSource = users;
        }

        List<string> users;
        public void LoadResults(string login)
        {
            stackPanel.Children.Clear();
            List<ResultPreview> resultsList = new List<ResultPreview>();

            SqlConnection connection = new SqlConnection("Data Source=DESKTOP-JOHEOHB\\SQLEXPRESS;Initial Catalog=dpl;User ID=sa;Password=12345");
            connection.Open();
            string command = "select Результаты.Идентификатор as Идентификатор, Тесты.Название as Тест, Шкалы.Название as Шкала, Пользователи.Имя as Имя, Пользователи.Фамилия as Фамилия, Пользователи.Логин as Логин, Значение, Максимум " +
                "from Результаты " +
                "join Пользователи on Пользователи.Идентификатор = Результаты.Идентификатор_пользователя " +
                "join Тесты on Тесты.Идентификатор = Результаты.Идентификатор_теста " +
                "join Шкалы on Тесты.Идентификатор_шкалы = Шкалы.Идентификатор " +
                "where Логин = '"+login+"'";
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
                stackPanel.Children.Add(label);
                return;
            }

            Dictionary<string, List<int>> percentages = new Dictionary<string, List<int>>();
            foreach (ResultPreview result in resultsList)
            {
                if (!percentages.ContainsKey(result.Scale))
                    percentages.Add(result.Scale, new List<int>());
                percentages[result.Scale].Add(result.Value*100 / result.Maximum);
            }
            Dictionary<string, int> percentage = new Dictionary<string, int>();
            foreach (string result in percentages.Keys)
            {
                int scaleSum = 0;
                foreach (int res in percentages[result])
                {
                    scaleSum += res;
                }
                int scaleResult = scaleSum / percentages[result].Count;
                percentage.Add(result, scaleSum);
            }

            Dictionary<string, int> scaleSummary = new Dictionary<string, int>();
            foreach (string key in percentages.Keys)
            {
                int summary = 0;
                foreach (int value in percentages[key])
                {
                    summary += value;
                }
                summary /= percentages[key].Count;
                scaleSummary.Add(key, summary);
            }

            command = "select Отделы.Название as Отдел, Должности.Название as Должность, Шкалы.Название as Шкала, Требования_шкал.Требования as Требования from Должности " +
                "join Требования_шкал on Требования_шкал.Идентификатор_должности = Должности.Идентификатор " +
                "join Шкалы on Требования_шкал.Идентификатор_шкалы = Шкалы.Идентификатор " +
                "join Отделы on Должности.Идентификатор_отдела = Отделы.Идентификатор";
            //выбрать все должности с отделами, собрать в словарь необходимые для них шкалы. показать принадлежность как среднее арифметическое перцентажа всех СУЩЕСТВУЮЩИХ результатов шкал, указать полноту в виде процента шкал, которые существуют
            reader = new SqlCommand(command, connection).ExecuteReader();
            Dictionary<string, List<ScaleContext>> prescriptionContext = new Dictionary<string, List<ScaleContext>>();
            while (reader.Read())
            {
                string prescriptionName = reader.GetString(reader.GetOrdinal("Отдел")) + "\n" + reader.GetString(reader.GetOrdinal("Должность"));
                if (!prescriptionContext.ContainsKey(prescriptionName))
                {
                    prescriptionContext.Add(prescriptionName, new List<ScaleContext>());
                }
                prescriptionContext[prescriptionName].Add(new ScaleContext(reader.GetString(reader.GetOrdinal("Шкала")), reader.GetInt32(reader.GetOrdinal("Требования"))));
            }
            reader.Close();
            connection.Close();
            Dictionary<string, int[]> prescriptionResults = new Dictionary<string, int[]>();
            foreach(string key in prescriptionContext.Keys)
            {
                //percentage - результаты тестов, ключ - шкала, значение - список процентажей тестов с этой шкалой
                int scalesCount = prescriptionContext[key].Count;
                int scalesGiven = 0;
                prescriptionResults.Add(key, new int[2]);//0 индекс массива - результат, 1 индекс - полнота
                
                foreach (ScaleContext scaleCon in prescriptionContext[key])//scales - список шкал (название+требование)
                {
                    string scale = scaleCon.Name;
                    if (percentage.ContainsKey(scale))
                    {
                        scalesGiven++;
                        prescriptionResults[key][0] += percentage[scale] * 100 / scaleCon.Percentage;//доля набранного результата от необходимого
                    }
                }
                prescriptionResults[key][0] /= scalesGiven;
                prescriptionResults[key][1] = scalesGiven * 100 / scalesCount;
            }
            foreach (string key in prescriptionResults.Keys)
            {
                int[] result = prescriptionResults[key];
                Border border = new Border();
                border.BorderThickness = new Thickness(1);
                border.BorderBrush = Brushes.Black;
                border.Margin = new Thickness(10);

                TextBlock resultText = new TextBlock();
                resultText.Text = key + "\nУровень знаний: " + result[0] + "%\nПолнота информации: " + result[1] + "%";
                resultText.Height = 65;
                resultText.Margin = new Thickness(10);

                border.Child = resultText;
                stackPanel.Children.Add(border);
            }
        }
        class ScaleContext
        {
            public string Name;
            public int    Percentage;
            public ScaleContext(string name, int percentage)
            {
                Name       = name;
                Percentage = percentage;
            }
        }

        private void UserComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string login = UserComboBox.SelectedItem.ToString().Split('(')[1].Trim(')');
            LoadResults(login);
        }
    }
}
