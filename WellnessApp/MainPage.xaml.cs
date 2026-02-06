using Android.Hardware.Lights;
using System.Diagnostics.Metrics;
using static Android.Provider.ContactsContract.CommonDataKinds;
using static JetBrains.Annotations.Async;

namespace WellnessApp;

public partial class MainPage : ContentPage
{
    private string selectedGender = "Male";

    public MainPage()
    {
        InitializeComponent();
        HighlightGender();
        UpdateResults();
    }

    // Gender taps
    private void OnMaleTapped(object sender, EventArgs e)
    {
        selectedGender = "Male";
        HighlightGender();
        UpdateResults();
    }

    private void OnFemaleTapped(object sender, EventArgs e)
    {
        selectedGender = "Female";
        HighlightGender();
        UpdateResults();
    }

    private void HighlightGender()
    {
        bool isMale = selectedGender == "Male";

        MaleFrame.BackgroundColor = isMale ? Colors.LightBlue : Colors.Transparent;
        FemaleFrame.BackgroundColor = !isMale ? Colors.LightPink : Colors.Transparent;

        MaleFrame.BorderColor = isMale ? Colors.Blue : Colors.Gray;
        FemaleFrame.BorderColor = !isMale ? Colors.DeepPink : Colors.Gray;

        MaleFrame.Scale = isMale ? 1.05 : 1;
        FemaleFrame.Scale = !isMale ? 1.05 : 1;
    }

    // Slider updates
    private void OnValueChanged(object sender, ValueChangedEventArgs e)
    {
        SleepLabel.Text = $"{SleepSlider.Value:F1} h";
        StressLabel.Text = $"{StressSlider.Value:F0}";
        ActivityLabel.Text = $"{ActivitySlider.Value:F0} min";

        UpdateResults();
    }

    // Wellness calculation
    private void UpdateResults()
    {
        double rawScore =
            (SleepSlider.Value * 8) -
            (StressSlider.Value * 5) +
            (ActivitySlider.Value * 0.5);

        rawScore = Math.Max(0, Math.Min(100, rawScore));
        int finalScore = (int)Math.Round(rawScore);

        ScoreLabel.Text = finalScore.ToString();

        string status = GetStatus(finalScore);
        StatusLabel.Text = status;
        StatusLabel.TextColor = GetStatusColor(status);

        RecommendationLabel.Text = GetRecommendation(status);
    }

    private string GetStatus(int score)
    {
        if (score >= 80) return "Excellent";
        if (score >= 60) return "Good";
        if (score >= 40) return "Fair";
        return "Poor";
    }

    private Color GetStatusColor(string status) =>
        status switch
        {
            "Excellent" => Colors.Green,
            "Good" => Colors.DarkOliveGreen,
            "Fair" => Colors.Orange,
            "Poor" => Colors.Red
        };

    private string GetRecommendation(string status)
    {
        if (selectedGender == "Male")
        {
            return status switch
            {
                "Excellent" => "Maintain routine; include resistance training 2–3× per week; ensure protein intake across meals.",
                "Good" => "Improve recovery with an earlier bedtime; add 15 min of light cardio or stretching; keep hydration steady.",
                "Fair" => "Aim for +1 hour of sleep; reduce caffeine after noon; schedule light mobility or an easy walk.",
                "Poor"=> "Rest today; avoid strenuous workouts; focus on hydration and 20–30 min of gentle walking."
            };
        }
        else
        {
            return status switch
            {
                "Excellent" => "Keep strong habits; add yoga/pilates for recovery; prioritize calcium + vitamin D intake.",
                "Good" => "Boost energy with a balanced breakfast; add 15 min of walking; focus on iron-rich foods if feeling low.",
                "Fair" => "Increase sleep consistency; reduce evening screen time; include calming routines like meditation or journaling.",
                "Poor" => "Prioritize rest and self-care; consider a short nap if possible; gentle yoga/stretching only."
            };
        }
    }
}

