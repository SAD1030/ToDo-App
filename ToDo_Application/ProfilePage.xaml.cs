namespace ToDo_Application;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
    }

    private async void OnSignOutClicked(object? sender, EventArgs e)
    {
        bool answer = await this.DisplayAlert("Sign Out", "Are you sure you want to sign out?", "Yes", "No");
        
        if (answer && Application.Current?.Windows.Count > 0)
        {
            // Reset to the login navigation stack
            Application.Current.Windows[0].Page = new NavigationPage(new SignInPage());
        }
    }
}