using PasswordVault.ViewModels;

namespace PasswordVault.Views;

public partial class VaultPage : ContentPage
{
    private readonly VaultViewModel _viewModel;

    public VaultPage(VaultViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadAsync();
    }
}