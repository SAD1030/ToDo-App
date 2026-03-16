namespace ToDo_Application;

[QueryProperty(nameof(ItemId), "id")]
public partial class EditToDoPage : ContentPage
{
    private ToDoClass? _currentItem;

    public string ItemId
    {
        set
        {
            if (int.TryParse(value, out int id))
            {
                _currentItem = ToDoPage.ToDoList.FirstOrDefault(x => x.item_id == id);
                if (_currentItem != null)
                {
                    TitleEntry.Text = _currentItem.item_name;
                    DetailsEditor.Text = _currentItem.item_description;
                }
            }
        }
    }

    public EditToDoPage() { InitializeComponent(); }

    private async void OnUpdateClicked(object? sender, EventArgs e)
    {
        if (_currentItem != null)
        {
            _currentItem.item_name = TitleEntry.Text;
            _currentItem.item_description = DetailsEditor.Text;
        }
        await Shell.Current.GoToAsync("..");
    }

    private async void OnCompleteClicked(object? sender, EventArgs e)
    {
        if (_currentItem != null) { _currentItem.status = "Completed"; }
        await Shell.Current.GoToAsync("..");
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        if (_currentItem != null) { ToDoPage.ToDoList.Remove(_currentItem); }
        await Shell.Current.GoToAsync("..");
    }
}