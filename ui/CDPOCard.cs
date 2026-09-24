using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace View.Ui
{
    public static class CDPOCard
    {
        public static Border Card(FrameworkElement content) => new()
        {
            Background = Theme.Card,
            CornerRadius = Theme.Radius,
            Padding = new Thickness(20),
            Child = content,
            Effect = new DropShadowEffect
            {
                BlurRadius = 24,
                ShadowDepth = 0,
                Opacity = 0.25,
                Color = Colors.Black
            }
        };

        public static Border Spacer(double height) => new() { Height = height };
    }
}