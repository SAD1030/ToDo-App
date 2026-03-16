namespace ToDo_Application;

public partial class SignInPage : ContentPage
{
    public SignInPage()
    {
        InitializeComponent();
    }

    private void OnSignInClicked(object? sender, EventArgs e)
    {
        // Modern .NET 10 way to change the root page
        if (Application.Current?.Windows.Count > 0)
        {
            Application.Current.Windows[0].Page = new AppShell();
        }
    }

    private async void OnSignUpClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage());
    }
}