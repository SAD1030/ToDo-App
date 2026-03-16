using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToDo_Application;

public class ToDoClass : INotifyPropertyChanged
{
    private string _item_name = string.Empty;
    private string _item_description = string.Empty;
    private string _status = "Incomplete";

    public int item_id { get; set; }

    public string item_name
    {
        get => _item_name;
        set { _item_name = value; OnPropertyChanged(); }
    }

    public string item_description
    {
        get => _item_description;
        set { _item_description = value; OnPropertyChanged(); }
    }

    public string status
    {
        get => _status;
        set { _status = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}