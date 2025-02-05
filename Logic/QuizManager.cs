using HamStudyX.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

//<summary>
//All user interaction will need to occur within the MAUI pages.
//Keep the logic for loading Qs, storing and shuffling Qs.
//Remove Console...
//</summary>

namespace HamStudyX.Logic
{
    /// <summary>
    /// Manages the quiz process: loading questions, starting quiz, shuffling, and scoring.
    /// Uses a Dictionary to store Question objects.
    /// Demonstrates OOP structure and usage of a collection.
    /// </summary>
    public class QuizManager
    {
        // Add property to track the user's score
        public int Score { get; set; } = 0;

        // Property to get the total number of questions
        public int TotalQuestions => _questions.Count;

        // Add method to reset the score
        public void ResetScore()
        {
            Score = 0;
        }

        // Add method to increment the score
        public void IncrementScore()
        {
            Score++;
        }

        // A dictionary where key is the Question's Id and value is the Question object
        private Dictionary<int, Question> _questions;

        // Constructor initializes the dictionary
        public QuizManager()
        {
            //Had NullReferenceException before
            _questions = new Dictionary<int, Question>();
        }

        /// <summary>
        /// Loads questions from a JSON file, creates Question or MultipleChoiceQuestion objects
        /// and stores them in the dictionary.
        /// </summary>
        public void LoadQuestions(List<RawQuestion> rawQuestions)
        {
            try
            {
                // Check validity of topic. Exists?
                if (rawQuestions != null || rawQuestions?.Count > 0)
                {
                    
                    // Iterate over each RawQuestion in the selected topic
                    foreach (var rq in rawQuestions)
                    {
                        if (rq == null || rq.Prompt == null || rq.Answer == null)
                        {
                            throw new Exception("Invalid question data.");
                        }

                        Question q;

                        // Check type to determine which class to instantiate
                        //Instantiates Q or MC object based on type & adds q to _questions with ID as key
                        if (rq.Type != null && rq.Type.Equals("MultipleChoice", StringComparison.OrdinalIgnoreCase))
                        {
                            if (rq.Options == null)
                            {
                                throw new Exception("MultipleChoice question missing options.");
                            }
                            q = new MultipleChoiceQuestion(rq.Id, rq.Prompt, rq.Answer, new List<string>(rq.Options));
                        }
                        else
                        {
                            q = new Question(rq.Id, rq.Prompt, rq.Answer);
                        }

                        _questions.Add(rq.Id, q);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading questions: {ex.Message}");
            }
        }

        public List<Question> GetAllQuestions()
        {
            // Return a list of the loaded questions
            return _questions.Values.ToList();
        }

        /// <summary>
        /// Method to shuffle a list of items using the Fisher-Yates algorithm.
        /// </summary>
        public void Shuffle<T>(List<T> list)
        {
            Random rand = new Random();
            for (int i = 0; i < list.Count; i++)
            {
                int randomIndex = rand.Next(i, list.Count);
                T temp = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }
    }
}
