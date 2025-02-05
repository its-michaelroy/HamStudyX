using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HamStudyX.Logic
{
    /// <summary>
    /// Represents a multiple-choice question (mcq).
    /// Inherits from the base Question class and adds a list of options.
    /// </summary>
    public class MultipleChoiceQuestion : Question
    {
        // List of possible answers for the multiple-choice question.
        public List<string> Options { get; set; }
        [SetsRequiredMembers]

        /// <summary>
        /// Initializes a new instance of the MultipleChoiceQuestion class.
        /// ID, prompt, answer, and options.
        /// </summary>
        public MultipleChoiceQuestion(int id, string prompt, string answer, List<string> options)
                : base(id, prompt, answer)
        {
            Options = options;
        }

        /// <summary>
        /// Checks the user's answer.
        /// Overrides the base class method Q.CheckAnswer
        /// </summary>
        public override bool CheckAnswer(string userResponse)
        {
            // First, check if user entered a number cons
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