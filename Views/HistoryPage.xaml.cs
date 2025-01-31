using System.Collections.ObjectModel;
using HamStudyX.Views;

namespace HamStudyX.Views;

public partial class HistoryPage : ContentPage
{
    public ObservableCollection<QuizHistoryItem> QuizHistory { get; set; } = new();

    public HistoryPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadQuizHistory();
    }

    private void LoadQuizHistory()
    {
        // Retrieve quiz history from persist storage (Sqlite, MySQL, etc.)
        // Use Pref..etc above mechs ^
        // QuizHistory == Load from sto
    }
}

public class QuizHistoryItem
{
    public DateTime DateTaken { get; set; }
    public string? Topic { get; set; }
    public double ScorePercentage { get; set; }
}
