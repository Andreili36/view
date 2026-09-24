using System.Windows;
using System.Windows.Controls;
using View.Modules.PlatformModule;
using View.Modules.PlatformModule.Sections;

namespace View.Modules.MainModule
{
    public class MainMenu
    {
        private readonly PlatformMain _host;
        private readonly List<SectionBase> _sections;

        public MainMenu(PlatformMain host, List<SectionBase> sections)
        {
            _host = host;
            _sections = sections;
        }

        public FrameworkElement Build()
        {
            var panel = new StackPanel { Margin = new Thickness(0) };

            panel.Children.Add(new TextBlock
            {
                Text = _host.UserName,
                FontSize = 15,
                FontWeight = FontWeights.SemiBold,
                Foreground = Ui.Theme.Text,
                FontFamily = Ui.Theme.Font,
                Margin = new Thickness(8, 0, 0, 2)
            });

            panel.Children.Add(new TextBlock
            {
                Text = RoleName(_host.RoleId),
                FontSize = 12,
                Foreground = Ui.Theme.TextDim,
                FontFamily = Ui.Theme.Font,
                Margin = new Thickness(8, 0, 0, 24)
            });

            foreach (var s in _sections.Where(x => x.IsVisible))
            {
                var section = s;
                var btn = Ui.CDPOButton.Menu(section.Title);
                btn.Click += (_, _) => _host.NavigateTo(section);
                panel.Children.Add(btn);
            }

            var logout = Ui.CDPOButton.Menu("Выход");
            logout.Foreground = Ui.Theme.Danger;
            logout.Click += (_, _) => Application.Current.Shutdown();
            panel.Children.Add(logout);

            return panel;
        }

        private static string RoleName(int role) => role switch
        {
            1 => "Администратор",
            2 => "Пользователь",
            3 => "Гость",
            _ => "Неизвестно"
        };
    }
}