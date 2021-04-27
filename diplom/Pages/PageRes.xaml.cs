using System.Windows.Controls;

namespace diplom.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageRes.xaml
    /// </summary>
    public partial class PageRes : Page
    {
        public PageRes()
        {
            InitializeComponent();
        }
        public void ShowResult(int value)
        {
            LabelMessage.Content = "Вы успешно завершили тест\nНабрано баллов: " + value;
        }
    }
}
