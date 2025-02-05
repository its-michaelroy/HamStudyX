using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace HamStudyX.Models
{
    public class QuizHistoryItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime DateTaken { get; set; }
        public string? Topic { get; set; }
        public double ScorePercentage { get; set; }
    }
}
