using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace ToDo_Application;

[QueryProperty(nameof(ItemId), "id")]
public partial class EditToDoPage : ContentPage
{
    private ToDoClass? _currentItem;
    private readonly ApiService _apiService = new ApiService();

    public string ItemId
    {
        set
        {
            if (int.TryParse(value, out int id))
            {
                _currentItem = ToDoPage.ToDoList.FirstOrDefault(x => x.item_id == id);
                if (_currentItem != null)
                {
                    TitleEntry.Text = _currentItem.Title;
                    DetailsEditor.Text = _currentItem.Details;
                }
            }
        }
    }

    public EditToDoPage() 
    { 
        InitializeComponent(); 
    }

    private async void OnUpdateClicked(object? sender, EventArgs e)
    {
        try
        {
            if (_currentItem == null) return;

            string newTitle = TitleEntry.Text;
            string newDescription = DetailsEditor.Text;

            bool success = await _apiService.UpdateTaskAsync(_currentItem.item_id, newTitle, newDescription);
            
            if (success) await Shell.Current.GoToAsync("..");
            else await DisplayAlertAsync("Error", "Failed to update task.", "OK");
        }
        catch (Exception ex) // FIXED: Removed "System."
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private async void OnCompleteClicked(object? sender, EventArgs e)
    {
        try
        {
            if (_currentItem == null) return;

            bool success = await _apiService.ChangeStatusAsync(_currentItem.item_id, "inactive");
            
            if (success) await Shell.Current.GoToAsync("..");
            else await DisplayAlertAsync("Error", "Failed to mark task as complete.", "OK");
        }
        catch (Exception ex) // FIXED: Removed "System."
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        try
        {
            if (_currentItem == null) return;

            bool success = await _apiService.DeleteTaskAsync(_currentItem.item_id);
            
            if (success) await Shell.Current.GoToAsync("..");
            else await DisplayAlertAsync("Error", "Failed to delete task.", "OK");
        }
        catch (Exception ex) // FIXED: Removed "System."
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }
}