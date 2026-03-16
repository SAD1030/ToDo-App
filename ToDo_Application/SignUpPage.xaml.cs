namespace ToDo_Application;

public partial class SignUpPage : ContentPage
{
    public SignUpPage()
    {
        InitializeComponent();
    }

    private async void OnSignUpClicked(object? sender, EventArgs e)
    {
        // Modern .NET 10 way to call DisplayAlert (Async version)
        await this.DisplayAlert("Success", "Account created!", "OK");
        await Navigation.PopAsync();
    }

    private async void OnSignInClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}