using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace View.Ui
{
    public static class CDPOInput
    {
        public static TextBox Text(string value = "") => new()
        {
            Text = value,
            Height = 38,
            Padding = new Thickness(12, 8, 12, 8),
            FontSize = 14,
            FontFamily = Theme.Font,
            Background = Theme.Card,
            Foreground = Theme.Text,
            BorderBrush = Theme.Border,
            BorderThickness = new Thickness(1),
            CaretBrush = Theme.Text,
            VerticalContentAlignment = VerticalAlignment.Center
        };

        public static PasswordBox Password() => new()
        {
            Height = 38,
            Padding = new Thickness(12, 8, 12, 8),
            FontSize = 14,
            FontFamily = Theme.Font,
            Background = Theme.Card,
            Foreground = Theme.Text,
            BorderBrush = Theme.Border,
            BorderThickness = new Thickness(1),
            VerticalContentAlignment = VerticalAlignment.Center
        };
    }
}