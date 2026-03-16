using System.Collections.ObjectModel;

namespace ToDo_Application;

public partial class ToDoPage : ContentPage
{
    public static ObservableCollection<ToDoClass> ToDoList { get; set; } = new ObservableCollection<ToDoClass>();

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
        new ObservableCollection<ToDoClass>(ToDoList.Where(x => x.status == "Incomplete"));
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        OnPropertyChanged(nameof(IncompleteTasks));
    }
}