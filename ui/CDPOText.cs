using System.Windows;
using System.Windows.Controls;

namespace View.Ui
{
    public static class CDPOText
    {
        public static TextBlock H1(string text) => new()
        {
            Text = text,
            FontSize = 26,
            FontWeight = FontWeights.SemiBold,
            Foreground = Theme.Text,
            FontFamily = Theme.Font,
            Margin = new Thickness(0, 0, 0, 16)
        };

        public static TextBlock H2(string text) => new()
        {
            Text = text,
            FontSize = 15,
            FontWeight = FontWeights.SemiBold,
            Foreground = Theme.Text,
            FontFamily = Theme.Font
        };

        public static TextBlock Label(string text) => new()
        {
            Text = text,
            FontSize = 13,
            Foreground = Theme.TextDim,
            FontFamily = Theme.Font,
            Margin = new Thickness(0, 0, 0, 6)
        };

        public static TextBlock Dim(string text) => new()
        {
            Text = text,
            FontSize = 12,
            Foreground = Theme.TextDim,
            FontFamily = Theme.Font
        };
    }
}