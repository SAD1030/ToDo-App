using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage; // ADDED: So we can read the user ID!

namespace ToDo_Application;

[QueryProperty(nameof(ItemId), "id")]
public partial class EditCompletedPage : ContentPage
{
    private ToDoClass? _currentItem;
    private readonly ApiService _apiService = new ApiService();

    public string ItemId
    {
        set
        {
            if (int.TryParse(value, out int id))
            {
                LoadTaskDetails(id);
            }
        }
    }

    public EditCompletedPage() 
    { 
        InitializeComponent(); 
    }

    private async void LoadTaskDetails(int id)
    {
        // FIXED: Removed the hardcoded '3' and replaced it with your actual ID!
        int currentUserId = Preferences.Default.Get("current_user_id", 0);
        var tasks = await _apiService.GetTasksAsync(currentUserId, "inactive"); 
        _currentItem = tasks.FirstOrDefault(x => x.item_id == id);

        if (_currentItem != null)
        {
            // FIXED: Updated names to Title and Details!
            TitleEntry.Text = _currentItem.Title;
            DetailsEditor.Text = _currentItem.Details;
        }
    }

    private async void OnUpdateClicked(object? sender, EventArgs e)
    {
        if (_currentItem == null) return;

        string newTitle = TitleEntry.Text;
        string newDescription = DetailsEditor.Text ?? "";

        bool success = await _apiService.UpdateTaskAsync(_currentItem.item_id, newTitle, newDescription);
        
        if (success) await Shell.Current.GoToAsync("..");
        else await DisplayAlertAsync("Error", "Failed to update task.", "OK"); // FIXED warning
    }

    private async void OnRestoreClicked(object? sender, EventArgs e)
    {
        if (_currentItem == null) return;

        bool success = await _apiService.ChangeStatusAsync(_currentItem.item_id, "active");
        
        if (success) await Shell.Current.GoToAsync("..");
        else await DisplayAlertAsync("Error", "Failed to restore task.", "OK"); // FIXED warning
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        if (_currentItem == null) return;

        bool success = await _apiService.DeleteTaskAsync(_currentItem.item_id);
        
        if (success) await Shell.Current.GoToAsync("..");
        else await DisplayAlertAsync("Error", "Failed to delete task.", "OK"); // FIXED warning
    }
}