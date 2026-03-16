namespace ToDo_Application;

public partial class AddToDoPage : ContentPage
{
    public AddToDoPage()
    {
        InitializeComponent();
    }

    private async void OnSaveButtonClicked(object? sender, EventArgs e)
    {
        var newItem = new ToDoClass
        {
            item_id = ToDoPage.ToDoList.Count + 1,
            item_name = TitleEntry.Text ?? "New Task",
            item_description = DetailsEditor.Text ?? "",
            status = "Incomplete"
        };

        ToDoPage.ToDoList.Add(newItem);
        await Shell.Current.GoToAsync("..");
    }
}