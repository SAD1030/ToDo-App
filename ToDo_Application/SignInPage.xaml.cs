using System.Text.Json;

namespace ToDo_Application;

public partial class SignInPage : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();
    private string baseUrl = "https://todo-list.dcism.org";

    public SignInPage()
    {
        InitializeComponent();
    }

    private async void OnSignInClicked(object? sender, EventArgs e)
    {
        string email = emailEntry.Text;
        string password = passwordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Please enter email and password", "OK");
            return;
        }

        try
        {
            string url = $"{baseUrl}/signin_action.php?email={email}&password={password}";

            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            var result = JsonDocument.Parse(json);

            int status = result.RootElement.GetProperty("status").GetInt32();

            if (status == 200)
            {
                var user = result.RootElement.GetProperty("data");
                int userId = user.GetProperty("id").GetInt32();

                // OPTIONAL: store userId globally
                Preferences.Set("user_id", userId);

                await DisplayAlert("Success", "Login successful!", "OK");

                if (Application.Current?.Windows.Count > 0)
                {
                    Application.Current.Windows[0].Page = new AppShell();
                }
            }
            else
            {
                string message = result.RootElement.GetProperty("message").GetString();
                await DisplayAlert("Error", message, "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnSignUpClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage());
    }
}
