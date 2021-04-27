using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom
{
    class Test
    {
        public Test() { }
        public Test(int id) 
        {
            this.id = id;        
        }
        public Test(int id, string description, string scale) 
        {
            this.id          = id;
            this.Description = description;
            this.Scale       = scale;
        }
        public Test LoadContext()
        {
            SqlConnection connection = new SqlConnection("Data Source=DESKTOP-JOHEOHB\\SQLEXPRESS;Initial Catalog=dpl;User ID=sa;Password=12345");
            connection.Open();
            SqlDataReader reader = new SqlCommand("select * from Вопросы where Идентификатор_теста = "+id, connection).ExecuteReader();
            questions = new List<Question>();
            while (reader.Read())
            {
                int      questionID   = reader.GetInt32(reader.GetOrdinal("Идентификатор"));
                string   questionText = reader.GetString(reader.GetOrdinal("Текст"));
                Question question     = new Question(/*questionID,*/ questionText);

                List<Answer> answers  = new List<Answer>();
                SqlConnection connection2 = new SqlConnection("Data Source=DESKTOP-JOHEOHB\\SQLEXPRESS;Initial Catalog=dpl;User ID=sa;Password=12345");
                connection2.Open();
                SqlDataReader reader2 = new SqlCommand("select * from Ответы where Идентификатор_вопроса = " + questionID, connection2).ExecuteReader();
                while (reader2.Read())
                {
                    string answerText  = reader2.GetString(reader2.GetOrdinal("Текст"));
                    int    answerValue = reader2.GetInt32(reader2.GetOrdinal("Стоимость"));
                    Answer answer      = new Answer(answerText, answerValue);
                    answers.Add(answer);
                }
                reader2.Close();
                question.SetAnswers(answers);
                questions.Add(question);
            }
            reader.Close();
            return this;
        }
        int id;
        public string Description { get; }
        public string Scale { get; }
        List<Question> questions;
        public List<Question> Questions { get { return questions; } }

        public int CountResult()
        {
            int res = 0;
            foreach (Question question in questions)
            {
                if (question.ChosenAnswer != null)
                    res += question.ChosenAnswer.Value;
            }
            return res;
        }
    }

    class TestPreview
    {
        public int Id;
        public string Name;
        public TestPreview(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    class Question
    {
        public Question(/*int id,*/ string text)
        {
            //this.id   = id;
            this.Text = text;
        }
        //int id;
        public string Text;
        public void SetAnswers(List<Answer> answers)
        {
            this.answers = answers;
        }
        public void GiveAnswer(int given)
        {
            chosenAnswer = given;
        }
        int chosenAnswer = -1;
        List<Answer> answers;
        public Answer ChosenAnswer { get { return chosenAnswer == -1 ? null : answers[chosenAnswer]; } }
        public int ChosenAnswerNumber { get { return chosenAnswer; } }
        public List<Answer> Answers { get { return answers; } } 
    }



    class Answer
    {
        public Answer(string text, int value)
        {
            this.Text  = text;
            this.Value = value;
        }
        public string Text;
        public int Value;
    }
}
