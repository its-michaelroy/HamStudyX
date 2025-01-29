using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// <summary>
// Class Inherits from Question & is for MC Questions
// </summary>
namespace HamStudyX.Logic
{
    //Multiple Choice questions
    //Inherit from Question & add list of opts
    public class MultipleChoiceQuestion : Question
    {
        public List<string> Options { get; set; }
        [SetsRequiredMembers]
        public MultipleChoiceQuestion(int id, string prompt, string answer, List<string> options)
                : base(id, prompt, answer)
        {
            Options = options;
        }

        /// <summary>
        /// Displays the question along with multiple choice options.
        /// Overrides the base class method.
        /// </summary>
        public override void DisplayQuestion()
        {
            Console.WriteLine(Prompt);
            for (int i = 0; i < Options.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {Options[i]}");
            }
        }

        /// <summary>
        /// Checks the user's answer.
        /// Users can enter the exact answer or the option number.
        /// Overrides the base class method Q.CheckAnswer
        /// </summary>
        public override bool CheckAnswer(string userResponse)
        {
            // First, check if user entered a number
            if (int.TryParse(userResponse, out int choiceNumber))
            {
                int index = choiceNumber - 1;
                if (index >= 0 && index < Options.Count)
                {
                    return Options[index].Equals(Answer, StringComparison.InvariantCultureIgnoreCase);
                }
            }

            // If user typed the answer directly, defaults back to base check
            return base.CheckAnswer(userResponse);
        }
    }
}