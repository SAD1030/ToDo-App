using System.Collections.ObjectModel;

namespace ToDo_Application;

public partial class CompletedPage : ContentPage
{
    public ObservableCollection<ToDoClass> CompletedTasks => 
        new ObservableCollection<ToDoClass>(ToDoPage.ToDoList.Where(x => x.status == "Completed"));

    public CompletedPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        OnPropertyChanged(nameof(CompletedTasks));
    }

    private async void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is ToDoClass selectedItem)
        {
            await Shell.Current.GoToAsync($"{nameof(EditCompletedPage)}?id={selectedItem.item_id}");
            if (sender is CollectionView collectionView)
                collectionView.SelectedItem = null;
        }
    }

    private void OnDeleteClicked(object? sender, EventArgs e)
    {
        if (sender is Button { CommandParameter: ToDoClass item })
        {
            ToDoPage.ToDoList.Remove(item);
            OnPropertyChanged(nameof(CompletedTasks));
        }
    }
}