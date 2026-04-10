using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage; // Added to access the saved ID

namespace ToDo_Application;

public partial class AddToDoPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();

    public AddToDoPage()
    {
        InitializeComponent();
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        string title = TitleEntry.Text;
        string description = DetailsEditor.Text ?? "";

        // Stop the user from saving a blank task
        if (string.IsNullOrWhiteSpace(title))
        {
            await DisplayAlertAsync("Error", "Please enter a task title.", "OK");
            return;
        }

        // Grabs the real ID of the person who just logged in!
        int currentUserId = Preferences.Default.Get("current_user_id", 0);

        if (currentUserId == 0)
        {
            await DisplayAlertAsync("Error", "You must be logged in to create a task.", "OK");
            return;
        }

        // FIXED: Changed to AddTaskAsync and put the parameters in the exact order the ApiService expects them!
        bool success = await _apiService.AddTaskAsync(title, description, currentUserId);

        if (success)
        {
            // Go back to the ToDo list page
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await DisplayAlertAsync("Error", "Failed to save task.", "OK");
        }
    }

    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}