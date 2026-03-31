using System.Text;
using System.Text.Json;

namespace ToDo_Application;

public partial class SignUpPage : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();
    private string baseUrl = "https://todo-list.dcism.org";

    public SignUpPage()
    {
        InitializeComponent();
    }

    private async void OnSignUpClicked(object? sender, EventArgs e)
    {
        var data = new
        {
            first_name = firstNameEntry.Text,
            last_name = lastNameEntry.Text,
            email = emailEntry.Text,
            password = passwordEntry.Text,
            confirm_password = confirmPasswordEntry.Text
        };

        try
        {
            string json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{baseUrl}/signup_action.php", content);
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonDocument.Parse(responseString);

            int status = result.RootElement.GetProperty("status").GetInt32();
            string message = result.RootElement.GetProperty("message").GetString();

            if (status == 200)
            {
                await DisplayAlert("Success", message, "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", message, "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnSignInClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
