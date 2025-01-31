using HamStudyX.Logic;
using System.Text.Json;
using HamStudyX.Views;


namespace HamStudyX.Views;

public partial class HomePage : ContentPage
{
    // Copy of the entire JSON data so we can easily switch topics.
    // Key: topic name, Value: list of RawQuestion objects
    private Dictionary<string, List<RawQuestion>>? _allTopics;

    public HomePage()
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