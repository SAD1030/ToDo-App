using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ToDo_Application;

public class GetTasksResponse
{
    [JsonPropertyName("status")] 
    public int Status { get; set; }

    [JsonPropertyName("data")] 
    public Dictionary<string, ToDoClass>? Data { get; set; }

    // FIXED: Changed to 'int' because the database sends a number, not text!
    [JsonPropertyName("count")] 
    public int Count { get; set; }
}

public class ToDoClass : INotifyPropertyChanged
{
    // Updated backing fields
    private string _title = string.Empty;
    private string _details = string.Empty;
    private string _status = "Incomplete";

    [JsonPropertyName("item_id")]
    public int item_id { get; set; }

    // FIXED: Renamed the C# variable back to Title so your XAML design can find it
    [JsonPropertyName("item_name")]
    public string Title
    {
        get => _title;
        set { _title = value; OnPropertyChanged(); }
    }

    // FIXED: Renamed the C# variable back to Details so your XAML design can find it
    [JsonPropertyName("item_description")]
    public string Details
    {
        get => _details;
        set { _details = value; OnPropertyChanged(); }
    }

    [JsonPropertyName("status")]
    public string status
    {
        get => _status;
        set { _status = value; OnPropertyChanged(); }
    }

    [JsonPropertyName("user_id")]
    public int user_id { get; set; }

    [JsonPropertyName("timemodified")]
    public string? timemodified { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}