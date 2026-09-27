using PasswordVault.ViewModels;

namespace PasswordVault.Views;

public partial class AddPasswordPage : ContentPage
{
    public AddPasswordPage(
        AddPasswordViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}