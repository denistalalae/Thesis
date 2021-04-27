namespace diplom.Pages
{
    class ResultPreview
    {
        public int Id;
        public string Test;
        public string Name;
        public string Surname;
        public string Login;
        public int Value;
        public int Maximum;
        public string Scale;
        public ResultPreview(int id, string test, string name, string surname, string login, int value, int maximum, string scale)
        {
            Id = id;
            Test = test;
            Name = name;
            Surname = surname;
            Login = login;
            Value = value;
            Maximum = maximum;
            Scale = scale;
        }
    }
}