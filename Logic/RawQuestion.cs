using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// <summary>
//Class to Deserialize questions from JSON file & hold raw data for each Q
// </summary>
namespace HamStudyX.Logic
{
    // A class used only to help deserialize the JSON structure
    public class RawQuestion
    {
        public int Id { get; set; }
        public string ?Type { get; set; }
        public string ?Prompt { get; set; }
        public string ?Answer { get; set; }
        public string[] ?Options { get; set; }
    }
}