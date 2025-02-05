using HamStudyX.Services;
using System.IO;

namespace HamStudyX
{
    public partial class App : Application
    {
        // Sing intance of db service
        static DatabaseService? _database;

        /// <summary>
        /// Access to db service instance.
        /// Init if not already.
        /// </summary>
        public static DatabaseService Database
        {
            get
            {
                if (_database == null)
                {
                    // Construct db path by combining db path with local app data folder.
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