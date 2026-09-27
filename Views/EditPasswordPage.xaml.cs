using PasswordVault.Models;
using PasswordVault.ViewModels;

namespace PasswordVault.Views;

public partial class EditPasswordPage : ContentPage
{
    private readonly EditPasswordViewModel _viewModel;

    public EditPasswordPage(EditPasswordViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public void LoadEntry(PasswordEntry entry)
    {
        _viewModel.LoadEntry(entry);
    }
}