using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;

namespace ToDo_Application;

public partial class ToDoPage : ContentPage
{
    // FIXED: The 'static' keyword is required here so EditToDoPage can find it!
    public static ObservableCollection<ToDoClass> ToDoList { get; set; } = new ObservableCollection<ToDoClass>();

    private readonly ApiService _apiService = new ApiService();

    public ToDoPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void OnAddButtonClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddToDoPage));
    }

    private async void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is ToDoClass selectedItem)
        {
            await Shell.Current.GoToAsync($"{nameof(EditToDoPage)}?id={selectedItem.item_id}");
            
            if (sender is CollectionView collectionView)
                collectionView.SelectedItem = null;
        }
    }
    
    public ObservableCollection<ToDoClass> IncompleteTasks => 
        new ObservableCollection<ToDoClass>(ToDoList.Where(x => x.status == "active" || x.status == "Incomplete"));
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        int currentUserId = Preferences.Default.Get("current_user_id", 0); 
    
        // Fetch the tasks
        List<ToDoClass> fetchedTasks = await _apiService.GetTasksAsync(currentUserId, "active");
        

        ToDoList.Clear();
        foreach (var task in fetchedTasks)
        {
            ToDoList.Add(task);
        }

        OnPropertyChanged(nameof(IncompleteTasks));
    }
}