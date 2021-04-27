using System.Collections.Generic;
using System.Windows.Controls;

namespace diplom.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageRedactor.xaml
    /// </summary>
    public partial class PageRedactor : Page
    {
        public PageRedactor()
        {
            InitializeComponent();
        }
        public void StartNew()
        {
            questions = new List<Question>();
            questions.Add(new Question(""));
            currentQuestion = 1;
        }
        int currentQuestion = 0;
        List<Question> questions = new List<Question>();
    }
}
