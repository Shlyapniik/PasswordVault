using PasswordVault.ViewModels;

namespace PasswordVault.Views;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel _viewModel;

    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnDisappearing()
    {
        MasterPasswordEntry.Unfocus();

#if ANDROID
        var activity = Platform.CurrentActivity;

        if (activity != null)
        {
            var inputMethodManager =
                activity.GetSystemService(
                    Android.Content.Context.InputMethodService)
                as Android.Views.InputMethods.InputMethodManager;

            if (MasterPasswordEntry.Handler?.PlatformView is Android.Views.View nativeView)
            {
                inputMethodManager?.HideSoftInputFromWindow(
                    nativeView.WindowToken,
                    Android.Views.InputMethods.HideSoftInputFlags.None);
            }
        }
#endif

        base.OnDisappearing();
    }
}