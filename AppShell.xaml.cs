namespace HamStudyX
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.QuizSessionPage), typeof(Views.QuizSessionPage));
        }
    }
}
