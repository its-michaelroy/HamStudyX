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
        public void LoadQuestions(string jsonContent, string selectedTopic)
        {
            try
            {
                // Read all text from the JSON file
                //string jsonString = File.ReadAllText(filePath);

                // Deserialize into a dictionary of topics
                var allTopics = JsonSerializer.Deserialize<Dictionary<string, List<RawQuestion>>>(jsonContent);

                // Check validity of topic. Exists?
                if (allTopics != null && allTopics.ContainsKey(selectedTopic))
                {
                    var rawQuestions = allTopics[selectedTopic];
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
                else
                {
                    //Convert to throws from WriteLines
                    throw new Exception($"Topic '{selectedTopic}' not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading questions: {ex.Message}");
            }
        }

        /// <summary>
        /// Starts the quiz, displays instructions, shuffles questions, and handles user input.
        /// </summary>
        //public void StartQuiz()
        //{
        //    // Extract keys from dictionary and shuffle Q IDS for randomized order
        //    var keys = new List<int>(_questions.Keys);
        //    Shuffle(keys);

        //    int score = 0;

        //    // Clear console, display Q, prompt user for answer, check answer, display result, prompt user to continue
        //    // Iterate over collection conatinig IDS of Qs
        //    foreach (int key in keys)
        //    {
        //        var question = _questions[key];
        //        Console.Clear();
        //        question.DisplayQuestion();

        //        Console.Write("\nYour answer: ");
        //        string userAnswer = Console.ReadLine();

        //        //Checks correctness of user's answer & increments score or revels answer
        //        if (question.CheckAnswer(userAnswer))
        //        {
        //            Console.WriteLine("Correct!");
        //            score++;
        //        }
        //        else
        //        {
        //            Console.WriteLine($"Incorrect! The correct answer is: {question.Answer}");
        //        }

        //        Console.WriteLine("\nPress Enter to continue...");
        //        Console.ReadLine();
        //    }

        //    double percentage = ((double)score / _questions.Count) * 100;
        //    //Calc score and display along w percentage
        //    Console.Clear();
        //    Console.WriteLine("Quiz Completed!");
        //    Console.WriteLine($"Your final score is {score} out of {_questions.Count}.");
        //    Console.WriteLine($"Percentage: {percentage:F2}%");
        //    Console.WriteLine("\n\nPress Enter to exit.");
        //    Console.ReadLine();
        //}

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
