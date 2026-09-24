using System.Windows;
using System.Windows.Media;

namespace View.Ui
{
    public static class Theme
    {
        public static readonly Brush Bg = new SolidColorBrush(Color.FromRgb(0x1E, 0x1E, 0x2E));
        public static readonly Brush Surface = new SolidColorBrush(Color.FromRgb(0x24, 0x24, 0x36));
        public static readonly Brush Card = new SolidColorBrush(Color.FromRgb(0x2D, 0x2D, 0x3E));
        public static readonly Brush Border = new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x50));
        public static readonly Brush Accent = new SolidColorBrush(Color.FromRgb(0x4F, 0x8C, 0xFF));
        public static readonly Brush AccentHov = new SolidColorBrush(Color.FromRgb(0x6B, 0xA0, 0xFF));
        public static readonly Brush Text = new SolidColorBrush(Color.FromRgb(0xE0, 0xE0, 0xEA));
        public static readonly Brush TextDim = new SolidColorBrush(Color.FromRgb(0x90, 0x90, 0xA8));
        public static readonly Brush Danger = new SolidColorBrush(Color.FromRgb(0xFF, 0x55, 0x66));
        public static readonly Brush Success = new SolidColorBrush(Color.FromRgb(0x4A, 0xDE, 0x80));

        public static readonly FontFamily Font = new("Segoe UI");
        public static readonly CornerRadius Radius = new(10);
    }
}