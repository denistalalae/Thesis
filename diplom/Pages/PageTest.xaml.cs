using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace diplom.Pages
{
    /// <summary>
    /// Логика взаимодействия для Test.xaml
    /// </summary>
    public partial class PageTest : Page
    {
        public PageTest()
        {
            InitializeComponent();
        }

        public void LoadTest(int testID)
        {
            DeselectAll();
            ButtonBack.IsEnabled = false;
            currentTest = new Test(testID).LoadContext();
            CurrentQuestion = 0;
        }


        Test currentTest;

        int currentQuestion;
        public int CurrentQuestion { get { return currentQuestion; } set { currentQuestion = value; LoadQuestion(currentTest.Questions[value]); } }
        int chosenAnswer;
        public int ChosenAnswer { get { return chosenAnswer; } set { chosenAnswer = value; currentTest.Questions[currentQuestion].GiveAnswer(value); } }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            currentTest.Questions[currentQuestion].GiveAnswer(chosenAnswer);
            CurrentQuestion -= 1;
            if (currentQuestion == 0)
            {
                ButtonBack.IsEnabled = false;
            }
            if (ButtonNext.Content.ToString() == "Закончить тест")
            {
                ButtonNext.Content = "Далее";
            }
            chosenAnswer = currentTest.Questions[currentQuestion].ChosenAnswerNumber;
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentQuestion == currentTest.Questions.Count-1)
            {
                EndTest();
                return;
            }
            CurrentQuestion += 1;
            ButtonBack.IsEnabled = true;
            if (currentQuestion == currentTest.Questions.Count-1)
            {
                ButtonNext.Content = "Закончить тест";
            }
            chosenAnswer = currentTest.Questions[currentQuestion].ChosenAnswerNumber;
        }

        private void EndTest()
        {
            int res = currentTest.CountResult();
            MainWindow.ShowTestResult(res);
        }

        private void LoadQuestion(Question question)
        {
            LabelQuestion.Content = question.Text;
            ButtonA1.Content = question.Answers[0].Text;
            ButtonA2.Content = question.Answers[1].Text;
            ButtonA3.Content = question.Answers[2].Text;
            ButtonA4.Content = question.Answers[3].Text;
            switch (question.ChosenAnswerNumber)
            {
                case -1: DeselectAll(); break;
                case 0:
                case 1:
                case 2:
                case 3: ChangeSelection(question.ChosenAnswerNumber); break;
            }
        }

        void DeselectAll()
        {
            ButtonA1.Background = new SolidColorBrush(Colors.White);
            ButtonA2.Background = new SolidColorBrush(Colors.White);
            ButtonA3.Background = new SolidColorBrush(Colors.White);
            ButtonA4.Background = new SolidColorBrush(Colors.White);
        }
        private void ChangeSelectionTo1(object sender, RoutedEventArgs e)
        {
            ChangeSelection(0);
        }
        private void ChangeSelectionTo2(object sender, RoutedEventArgs e)
        {
            ChangeSelection(1);
        }
        private void ChangeSelectionTo3(object sender, RoutedEventArgs e)
        {
            ChangeSelection(2);
        }
        private void ChangeSelectionTo4(object sender, RoutedEventArgs e)
        {
            ChangeSelection(3);
        }

        void ChangeSelection(int choise)
        {
            switch (choise)
            {
                case 0: DeselectAll(); ButtonA1.Background = new SolidColorBrush(Colors.Green); break;
                case 1: DeselectAll(); ButtonA2.Background = new SolidColorBrush(Colors.Green); break;
                case 2: DeselectAll(); ButtonA3.Background = new SolidColorBrush(Colors.Green); break;
                case 3: DeselectAll(); ButtonA4.Background = new SolidColorBrush(Colors.Green); break;
            }
            ChosenAnswer = choise;
        }
    }
}
