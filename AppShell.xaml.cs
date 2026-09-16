using PasswordVault.Views;

namespace PasswordVault;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(VaultPage), typeof(VaultPage));
        Routing.RegisterRoute(nameof(AddPasswordPage), typeof(AddPasswordPage));
        Routing.RegisterRoute(nameof(PasswordDetailsPage), typeof(PasswordDetailsPage));
        Routing.RegisterRoute(nameof(EditPasswordPage), typeof(EditPasswordPage));
    }
}