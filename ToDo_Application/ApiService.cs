using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace ToDo_Application;

public class ApiService
{
    private static readonly HttpClient _client = new HttpClient();
    private const string BaseUrl = "https://todo-list.dcism.org";

    // 1. GET: Fetch Tasks
    public async Task<List<ToDoClass>> GetTasksAsync(int userId, string status)
    {
        string url = $"{BaseUrl}/getItems_action.php?status={status}&user_id={userId}";

        try
        {
            HttpResponseMessage response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<GetTasksResponse>(jsonString, options);

                if (result?.Status == 200 && result.Data != null)
                {
                    return result.Data.Values.ToList();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"API Error: {ex.Message}");
        }

        return new List<ToDoClass>(); 
    }
    
    // 2. POST: Add a new Task
    public async Task<bool> AddTaskAsync(string name, string description, int userId)
    {
        string url = $"{BaseUrl}/addItem_action.php"; 

        var payload = new
        {
            item_name = name,
            item_description = description,
            user_id = userId
        };

        string jsonString = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await _client.PostAsync(url, content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"API POST Error: {ex.Message}");
            return false;
        }
    }
    
    // 3. PUT: Update an existing Task
    public async Task<bool> UpdateTaskAsync(int itemId, string name, string description)
    {
        string url = $"{BaseUrl}/editItem_action.php"; 

        var payload = new
        {
            item_id = itemId,
            item_name = name,
            item_description = description
        };

        string jsonString = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await _client.PutAsync(url, content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"API Update Error: {ex.Message}");
            return false;
        }
    }
    
    // 4. PUT: Change the status (Complete/Restore)
    public async Task<bool> ChangeStatusAsync(int itemId, string status)
    {
        // FIXED: The wiretap proved this capital 'I' is exactly what the server wanted!
        string url = $"{BaseUrl}/statusItem_action.php"; 

        var payload = new
        {
            item_id = itemId,
            status = status
        };

        string jsonString = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await _client.PutAsync(url, content);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    // 5. DELETE: Permanently erase a task
    public async Task<bool> DeleteTaskAsync(int itemId)
    {
        string url = $"{BaseUrl}/deleteItem_action.php?item_id={itemId}"; 

        try
        {
            HttpResponseMessage response = await _client.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }
}