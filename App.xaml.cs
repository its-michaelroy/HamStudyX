using HamStudyX.Services;
using System.IO;

namespace HamStudyX
{
    public partial class App : Application
    {
        static DatabaseService? _database;
        public static DatabaseService Database
        {
            get
            {
                if (_database == null)
                {
                    var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "QuizHistory.db3");
                    _database = new DatabaseService(dbPath);
                }
                return _database;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}