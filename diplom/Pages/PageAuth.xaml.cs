using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace diplom.Pages
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class PageAuth : Page
    {
        public PageAuth()
        {
            InitializeComponent();
            LoginTB.Text        = "admin";
            PasswordTB.Password = "admin";
        }

        private void LogIn(object sender, RoutedEventArgs e)
        {
            string login    = LoginTB.Text;
            string password = PasswordTB.Password;

            SqlConnection connection = new SqlConnection("Data Source=DESKTOP-JOHEOHB\\SQLEXPRESS;Initial Catalog=dpl;User ID=sa;Password=12345");
            connection.Open();
            SqlDataReader reader = new SqlCommand("select * from Пользователи where Логин = '" + login.Replace("\\", "\\\\").Replace("'", "''").Replace("\"", "\"\"") +"' and Пароль = '" + password.Replace("\\", "\\\\").Replace("'", "''").Replace("\"", "\"\"") + "'", connection).ExecuteReader();
            if (reader.Read())
            {
                MainWindow.CurrentLabelName.Foreground = Brushes.Black;
                MainWindow.SetLabelName("Здравствуйте, " + reader.GetString(reader.GetOrdinal("Фамилия")) + " " + reader.GetString(reader.GetOrdinal("Имя")) + " " + reader.GetString(reader.GetOrdinal("Отчество")));
                MainWindow.Role   = reader.GetString(reader.GetOrdinal("Роль"));
                MainWindow.UserId = reader.GetInt32(reader.GetOrdinal("Идентификатор"));
                MainWindow.Login  = reader.GetString(reader.GetOrdinal("Логин"));
                reader.Close();
                MainWindow.EnterMenu();
            }
            else
            {
                reader.Close();
                MainWindow.CurrentLabelName.Foreground = Brushes.Red;
                MainWindow.SetLabelName("Введены неправильные учетные данные");
            }
        }

        public void ClearCredentials()
        {
            MainWindow.SetLabelName("");
            LoginTB.Text        = "";
            PasswordTB.Password = "";
        }
    }
}
