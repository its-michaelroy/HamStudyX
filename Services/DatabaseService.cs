using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HamStudyX.Models;
using SQLite;

namespace HamStudyX.Services
{
    /// <summary>
    /// Provides methods to interact with the SQLite database for storing and retrieving quiz history.
    /// </summary>
    public class DatabaseService
    {
        // The SQLite connection
        readonly private SQLiteAsyncConnection _database;

        /// <summary>
        /// Initializes a new instance of the DatabaseService class.
        /// Creates the QuizHistoryItem table if it doesn't exist.
        /// </summary>
        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<QuizHistoryItem>().Wait();
        }

        /// <summary>
        /// Grabs all quiz history items from the database.
        /// </summary>
        public Task<List<QuizHistoryItem>> GetQuizHistoryAsync()
        {
            return _database.Table<QuizHistoryItem>().OrderByDescending(x => x.DateTaken).ToListAsync();
        }

        /// <summary>
        /// Saves a quiz history item to the database.
        /// </summary>
        public Task<int> SaveQuizHistoryItemAsync(QuizHistoryItem item)
        {
            return _database.InsertAsync(item);
        }
    }
}
