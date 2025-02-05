using HamStudyX.Logic;
using System.Text.Json;


namespace HamStudyX.Views;

public partial class HomePage : ContentPage
{
    // Copy of the entire JSON data so we can easily switch topics.
    // Dict to hold all topics & Qs. Key: topic name, Value: list of RawQuestion objects
    private Dictionary<string, List<RawQuestion>>? _allTopics;

    public HomePage()
    {
        InitializeComponent();
        LoadAllTopicsIntoPicker(); // Fill the Picker with topic names
    }

    /// <summary>
    /// Loads all topics from the JSON file and populates the Picker control.
    /// </summary>
    private async void LoadAllTopicsIntoPicker()
    {
        try
        {
            // Build path to JSON asset/open file async
            var jsonPath = await FileSystem.Current.OpenAppPackageFileAsync("QuestionSet.json");

            // Read it into a string
            using var reader = new StreamReader(jsonPath);
            string jsonContent = await reader.ReadToEndAsync();

            // Deserialize into a dictionary of topics
            _allTopics = JsonSerializer.Deserialize<Dictionary<string, List<RawQuestion>>>(jsonContent);

            if (_allTopics == null || _allTopics.Count == 0)
            {
                await DisplayAlert("Error", "No topics found in Question Set", "OK");
                return;
            }

            // Load the Picker with the topic names (the dictionary keys)
            foreach (var topic in _allTopics.Keys)
            {
                topicPicker.Items.Add(topic); //Connect to picker in xaml
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load topics: {ex.Message}", "OK");
        }
    }

    /// <summary>
    /// Event handler for the Load Quiz button.
    /// Navigates to the QuizSessionPage with the selected topic.
    /// </summary>
    private async void OnLoadQuizClicked(object sender, EventArgs e)
    {
        // Ensure user has picked a topic
        if (topicPicker.SelectedIndex == -1)
        {
            await DisplayAlert("Alert", "Please select a topic first.", "OK");
            return;
        }

        var selectedTopic = topicPicker.Items[topicPicker.SelectedIndex];

        // Ensure _allTopics is not null
        if (_allTopics == null || !_allTopics.ContainsKey(selectedTopic))
        {
            await DisplayAlert("Error", "Topics have not been loaded.", "OK");
            return;
        }

        // Navigate to QuizSessionPage and pass the selected topic
        await Shell.Current.GoToAsync(nameof(QuizSessionPage), new Dictionary<string, object>
        {
            { "SelectedTopic", selectedTopic },
            { "AllTopics", _allTopics }
        });
    }
}