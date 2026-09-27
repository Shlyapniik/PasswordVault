using PasswordVault.Models;
using PasswordVault.ViewModels;

namespace PasswordVault.Views;

public partial class PasswordDetailsPage : ContentPage
{
    private readonly PasswordDetailsViewModel _viewModel;

    public PasswordDetailsPage(
        PasswordDetailsViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public void LoadEntry(PasswordEntry entry)
    {
        _viewModel.LoadEntry(entry);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.Refresh();
    }
}