using HamStudyX.Logic;
using System.Text.Json;

namespace HamStudyX.Views;

// Allow passing params by query when navigating to this page
[QueryProperty(nameof(SelectedTopic), "SelectedTopic")]
[QueryProperty(nameof(AllTopics), "AllTopics")]

public partial class QuizSessionPage : ContentPage
{
    // Manages quiz logic like loading questions and scoring
    // List to hold all questions in current quiz session
    // Tracks index of current Question
    // Topic selected by user for the quiz
    // Lastly, all topics and related questions
    private QuizManager? _quizManager;
    private List<Question> _questions = new();
    private int _currentIndex = 0;
    public string? SelectedTopic { get; set; }
    public Dictionary<string, List<RawQuestion>>? AllTopics { get; set; }

    public QuizSessionPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Called when page appears. Inits quiz if not already done.
    /// Load Quiz if not already done
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_quizManager == null)
        {
            LoadQuiz();
        }
    }

    /// <summary>
    /// Loads quiz for user selectedtopic.
    /// Check if topic is chosen and Q is availabl
    /// </summary>
    private void LoadQuiz()
    {
        if (string.IsNullOrEmpty(SelectedTopic) || AllTopics == null)
        {
            return;
        }

        // Create new QuizManager and reset score
        _quizManager = new QuizManager();
        _quizManager.ResetScore();

        // Because QuizManager.LoadQuestions wants JSON text per topic,
        // we build a partial JSON just for the selected topic:
        //var selectedRaw = new Dictionary<string, List<RawQuestion>>
        //{
        //    { SelectedTopic, AllTopics[SelectedTopic] }
        //};
        //var partialJson = JsonSerializer.Serialize(selectedRaw);

        try
        {
            // Load the raw questions into QuizManager
            _quizManager.LoadQuestions(AllTopics[SelectedTopic]);

            // Get all questions from list
            _questions = _quizManager.GetAllQuestions();

            // Optional: shuffle them
            // You can also shuffle a separate copy if you prefer
            _quizManager.Shuffle(_questions);

            // Reset current question index
            _currentIndex = 0;

            // Update the score display
            scoreLabel.Text = $"Score: {_quizManager.Score}";

            // Show the first question
            ShowQuestion(_currentIndex);
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

    /// <summary>
    /// SHow Question at selected index.
    /// </summary>
    /// param name of index for question to display.
    private void ShowQuestion(int index)
    {
        if (_questions.Count == 0) return;
        if (index < 0 || index >= _questions.Count) return;

        // Clear out th previous UI elements for new Question
        questionLabel.Text = string.Empty;
        optionsStack.Children.Clear();
        
        userAnswerEntry.IsVisible = false;
        userAnswerEntry.Text = string.Empty;
        
        nextButton.IsVisible = true;
        
        feedbackLabel.IsVisible = false;
        feedbackLabel.Text = "";

        // Get the current question
        var question = _questions[index];
        questionLabel.Text = question.Prompt;

        // Check if it's a MultipleChoiceQuestion
        if (question is MultipleChoiceQuestion mcq)
        {
            // Create a button for each option
            for (int i = 0; i < mcq.Options.Count; i++)
            {
                var optionText = mcq.Options[i];
                var optionButton = new Button
                {
                    Text = $"{i + 1}) {optionText}",
                    HorizontalOptions = LayoutOptions.Start,
                    Style = (Style)Application.Current.Resources["AccentButtonStyle"]
                };

                // When option is clicked, store the users answer in userAnswerEntry
                // Update selected buttons appearance
                optionButton.Clicked += (s, e) =>
                {
                    userAnswerEntry.Text = optionText;
                    UpdateSelectedOptionButton(optionButton);
                };

                optionsStack.Children.Add(optionButton);
            }
        }
        else
        {
            // For a basic question, show the Entry for typed answers
            userAnswerEntry.IsVisible = true;
        }
    }

    // Store the selected button to update its appearance
    private Button? _selectedOptionButton;

    /// <summary>
    /// Update the appearance of the selected button.
    /// </summary>
    /// <param name="selectedButton"></param>
    private void UpdateSelectedOptionButton(Button selectedButton)
    {
        // Reset previously selected button's state
        if (_selectedOptionButton != null)
        {
            _selectedOptionButton.BackgroundColor = (Color)Application.Current.Resources["ButtonBackgroundColor"];
        }

        // Set/Highlight the new selected button's bg color
        _selectedOptionButton = selectedButton;
        _selectedOptionButton.BackgroundColor = (Color)Application.Current.Resources["AccentColor"];
    }

    /// <summary>
    /// Called when Next/Check/Verify button is clicked.
    /// Compare users answer to correct answer then proceeds.
    /// </summary>
    private async void OnNextClicked(object sender, EventArgs e)
    {
        // No questions loaded? Return.
        if (_questions == null || _questions.Count == 0) return;

        // Prevent proceeding w/o an answer
        if (string.IsNullOrWhiteSpace(userAnswerEntry.Text))
        {
            await DisplayAlert("Alert", "Please select or enter an answer.", "OK");
            return;
        }

        // Grab current question
        var question = _questions[_currentIndex];

        // The user's typed (or selected) answer
        string userResponse = userAnswerEntry.Text.Trim();

        // Evaluate correctness
        bool isCorrect = question.CheckAnswer(userResponse);

        // If correct, increment the score
        if (isCorrect && _quizManager != null)
        {
            _quizManager.IncrementScore();
        }

        // Update score on the UI/display
        if (_quizManager != null)
        {
            scoreLabel.Text = $"Score: {_quizManager.Score}";
        }

        // Provide feedback
        feedbackLabel.IsVisible = true;
        if (isCorrect)
        {
            feedbackLabel.Text = "Correct!";
            feedbackLabel.TextColor = (Color)Application.Current.Resources["SuccessColor"];
        }
        else
        {
            feedbackLabel.Text = $"Incorrect!\nThe correct answer is: {question.Answer}";
            feedbackLabel.TextColor = (Color)Application.Current.Resources["ErrorColor"];
        }

        // Wait for 2 seconds to see feedback
        await Task.Delay(TimeSpan.FromSeconds(2));

        // Hide feedback
        feedbackLabel.IsVisible = false;

        // Move to the next question
        _currentIndex++;

        if (_currentIndex < _questions.Count)
        {
            ShowQuestion(_currentIndex);
        }
        else
        {
            // No more questions since all Qs have been answered
            if (_quizManager != null)
            {
                double percentage = ((double)_quizManager.Score / _quizManager.TotalQuestions) * 100;
                await DisplayAlert("Quiz Complete",
                    $"You have answered all questions!\nYour final score is {_quizManager.Score} out of {_quizManager.TotalQuestions}.\nPercentage: {percentage:F2}%",
                    "OK");
            }
            else
            {
                await DisplayAlert("Quiz Complete", "You have answered all questions!", "OK");
            }
            nextButton.IsVisible = false;
        }
    }

    /// <summary>
    /// Called when Quit button is clicked and returns to Home Page.
    /// </summary>
    private async void OnQuitClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    /// <summary>
    /// Called when Restart button is clicked and resets quiz.
    /// Displays the first question again.
    /// </summary>
    private void OnRestartClicked(object sender, EventArgs e)
    {
        _currentIndex = 0;
        _quizManager?.ResetScore();
        scoreLabel.Text = $"Score: {_quizManager?.Score}";
        ShowQuestion(_currentIndex);
    }

    // CHANGE TO PERSIST STATE
    private async void OnQuizCompleted()
    {
        if (_quizManager != null)
        {
            double percentage = ((double)_quizManager.Score / _quizManager.TotalQuestions) * 100;
            await DisplayAlert("Quiz Complete",
                $"You have answered all questions!\nYour final score is {_quizManager.Score} out of {_quizManager.TotalQuestions}.\nPercentage: {percentage:F2}%",
                "OK");

            var quizResult = new QuizHistoryItem
            {
                DateTaken = DateTime.Now,
                Topic = SelectedTopic,
                ScorePercentage = percentage
            };

            // Save quizResult to persistent storage SQlite or Mysql or mariadb or cloud based?
            // using Preferences...etc (replace with actual storage mechanism above)
        }

        nextButton.IsVisible = false;
    }

}