using HamStudyX.Logic;
using System.Text.Json;
using Microsoft.Maui.Storage;
using System.Reflection.PortableExecutable;


namespace HamStudyX;

public partial class MainPage : ContentPage
{
    private QuizManager? _quizManager;
    private List<Question> _questions = new();
    private int _currentIndex = 0;

    // Copy of the entire JSON data so we can easily switch topics.
    // Key: topic name, Value: list of RawQuestion objects
    private Dictionary<string, List<RawQuestion>>? _allTopics;

    public MainPage()
    {
        InitializeComponent();
        LoadAllTopicsIntoPicker(); // Step 1: fill the Picker with topic names
    }

    private async void LoadAllTopicsIntoPicker()
    {
        try
        {
            // 1) Build the path to your JSON asset/open file async
            var jsonPath = await FileSystem.Current.OpenAppPackageFileAsync("QuestionSet.json");

            // 2) Read it into a string
            using var reader = new StreamReader(jsonPath);
            string jsonContent = await reader.ReadToEndAsync();

            // 3) Deserialize into a dictionary of topics
            _allTopics = JsonSerializer.Deserialize<Dictionary<string, List<RawQuestion>>>(jsonContent);

            if (_allTopics == null || _allTopics.Count == 0)
            {
                await DisplayAlert("Error", "No topics found in Question Set", "OK");
                return;
            }

            // 4) Populate the Picker with the topic names (the dictionary keys)
            foreach (var topic in _allTopics.Keys)
            {
                topicPicker.Items.Add(topic);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load topics: {ex.Message}", "OK");
        }
    }

    private void OnLoadQuizClicked(object sender, EventArgs e)
    {
        // Ensure user has picked a topic
        if (topicPicker.SelectedIndex == -1)
        {
            DisplayAlert("Alert", "Please select a topic first.", "OK");
            return;
        }

        var selectedTopic = topicPicker.Items[topicPicker.SelectedIndex];

        // Ensure _allTopics is not null
        if (_allTopics == null)
        {
            DisplayAlert("Error", "Topics have not been loaded.", "OK");
            return;
        }

        // Ensure _allTopics contains the selected topic
        if (!_allTopics.ContainsKey(selectedTopic))
        {
            DisplayAlert("Error", "Selected topic not found.", "OK");
            return;
        }

        // Create a brand new QuizManager
        _quizManager = new QuizManager();

        // Because QuizManager.LoadQuestions wants JSON text per topic,
        // we build a partial JSON just for the selected topic:
        var selectedRaw = new Dictionary<string, List<RawQuestion>>
        {
            { selectedTopic, _allTopics[selectedTopic] }
        };
        var partialJson = JsonSerializer.Serialize(selectedRaw);

        try
        {
            // Load the topic into QuizManager
            _quizManager.LoadQuestions(partialJson, selectedTopic);

            // Retrieve the question list
            _questions = _quizManager.GetAllQuestions();

            // Optional: shuffle them
            // You can also shuffle a separate copy if you prefer
            _quizManager.Shuffle(_questions);

            // Reset current question index
            _currentIndex = 0;

            // Show the first question
            ShowQuestion(_currentIndex);
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void ShowQuestion(int index)
    {
        if (_questions.Count == 0) return;
        if (index < 0 || index >= _questions.Count) return;

        // Clear out th previous UI
        questionLabel.Text = string.Empty;
        optionsStack.Children.Clear();
        userAnswerEntry.IsVisible = false;
        userAnswerEntry.Text = string.Empty;
        nextButton.IsVisible = true;

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
                    HorizontalOptions = LayoutOptions.Start
                };

                // When an option is clicked, store the user’s answer in userAnswerEntry
                optionButton.Clicked += (s, e) =>
                {
                    userAnswerEntry.Text = optionText;
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

    private void OnNextClicked(object sender, EventArgs e)
    {
        // No questions loaded? Return.
        if (_questions == null || _questions.Count == 0) return;

        // Grab the current question
        var question = _questions[_currentIndex];

        // The user's typed (or selected) answer
        string userResponse = userAnswerEntry.Text?.Trim() ?? "";

        // Evaluate correctness
        bool isCorrect = question.CheckAnswer(userResponse);

        // Provide feedback
        if (isCorrect)
        {
            DisplayAlert("Result", "Correct!", "OK");
        }
        else
        {
            DisplayAlert("Result", $"Incorrect! The correct answer is: {question.Answer}", "OK");
        }

        // Move to the next question
        _currentIndex++;

        if (_currentIndex < _questions.Count)
        {
            ShowQuestion(_currentIndex);
        }
        else
        {
            // No more questions
            DisplayAlert("Quiz Complete", "You have answered all questions!", "OK");
            nextButton.IsVisible = false;
        }
    }
}
