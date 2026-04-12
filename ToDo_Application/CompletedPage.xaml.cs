using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace ToDo_Application;

public partial class CompletedPage : ContentPage
{
    // The collection bound to your XAML list
    public ObservableCollection<ToDoClass> CompletedTasks { get; set; } = new ObservableCollection<ToDoClass>();
    
    // The service that talks to your PHP backend
    private readonly ApiService _apiService = new ApiService();

    public CompletedPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        int currentUserId = Preferences.Default.Get("current_user_id", 0);

        // Fetching "inactive" tasks to populate the completed list
        List<ToDoClass> fetchedTasks = await _apiService.GetTasksAsync(currentUserId, "inactive");

        CompletedTasks.Clear();
        foreach (var task in fetchedTasks)
        {
            CompletedTasks.Add(task);
        }
    }

    private async void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        // When a user taps a task, open the EditCompletedPage and pass the ID
        if (e.CurrentSelection.FirstOrDefault() is ToDoClass selectedItem)
        {
            await Shell.Current.GoToAsync($"{nameof(EditCompletedPage)}?id={selectedItem.item_id}");
            
            // Remove the gray highlight from the selected item
            if (sender is CollectionView collectionView)
                collectionView.SelectedItem = null;
        }
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        // When the inline trash can is clicked, figure out which task it belongs to
        if (sender is Button button && button.CommandParameter is ToDoClass itemToDelete)
        {
            // FIXED: Changed item_name to Title!
            bool confirm = await DisplayAlertAsync("Delete Task", $"Are you sure you want to permanently delete '{itemToDelete.Title}'?", "Yes", "No");

            if (confirm)
            {
                // Send the DELETE request to the database
                bool success = await _apiService.DeleteTaskAsync(itemToDelete.item_id);

                if (success)
                {
                    // Remove it from the screen immediately
                    CompletedTasks.Remove(itemToDelete);
                }
                else
                {
                    await DisplayAlertAsync("Error", "Failed to delete task from the server.", "OK");
                }
            }
        }
    }
}