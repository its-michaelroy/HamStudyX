using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HamStudyX.Models;
using SQLite;

namespace HamStudyX.Services
{
    public class DatabaseService
    {
        readonly private SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<QuizHistoryItem>().Wait();
        }

        public Task<List<QuizHistoryItem>> GetQuizHistoryAsync()
        {
            return _database.Table<QuizHistoryItem>().OrderByDescending(x => x.DateTaken).ToListAsync();
        }

        public Task<int> SaveQuizHistoryItemAsync(QuizHistoryItem item)
        {
            return _database.InsertAsync(item);
        }
    }
}
