using System.Windows;
using System.Windows.Controls;
using View.Data;
using View.Modules.AuthModule;
using View.Modules.PlatformModule;

namespace View;

public class MainForm : Window
{
    private readonly ContentControl _host = new();

    public DbWrapper Db { get; }

    public MainForm(DbWrapper db)
    {
        Db = db;

        Title = "Центр ДПО";
        Width = 1200;
        Height = 740;
        MinWidth = 900;
        MinHeight = 600;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        Background = Ui.Theme.Bg;
        Foreground = Ui.Theme.Text;
        FontFamily = Ui.Theme.Font;

        Content = _host;
        LoadAuthModule();
    }

    private void LoadAuthModule()
    {
        var auth = new AuthMain(this);
        auth.OnAuthorized += (roleId, userName) => LoadPlatformModule(roleId, userName);
        _host.Content = auth.Build();
    }

    private void LoadPlatformModule(int roleId, string userName)
    {
        var platform = new PlatformMain(this, roleId, userName);
        _host.Content = platform.Build();
    }

    public void ShowMessage(string text) =>
        MessageBox.Show(this, text, "Сообщение",
            MessageBoxButton.OK, MessageBoxImage.Information);

    public void ShowError(string text) =>
        MessageBox.Show(this, text, "Ошибка",
            MessageBoxButton.OK, MessageBoxImage.Error);

    public bool Confirm(string text) =>
        MessageBox.Show(this, text, "Подтверждение",
            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
}