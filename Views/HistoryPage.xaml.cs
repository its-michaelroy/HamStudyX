using System.Collections.ObjectModel;
using HamStudyX.Models;

namespace HamStudyX.Views;

public partial class HistoryPage : ContentPage
{
    public HistoryPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            var historyItems = await App.Database.GetQuizHistoryAsync();
            if (historyItems != null && historyItems.Count > 0)
            {
                historyListView.ItemsSource = new ObservableCollection<QuizHistoryItem>(historyItems);
            }
            else
            {
                await DisplayAlert("No History", "No quiz history available.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load quiz history: {ex.Message}", "OK");
        }
    }
}