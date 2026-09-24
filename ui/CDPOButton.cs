using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace View.Ui
{
    public static class CDPOButton
    {
        public static Button Menu(string text)
        {
            var b = Base(text, Theme.Card, Theme.Text);
            b.Height = 42;
            b.HorizontalContentAlignment = HorizontalAlignment.Left;
            b.Padding = new Thickness(16, 0, 0, 0);
            b.Margin = new Thickness(0, 4, 0, 4);
            b.Template = FlatTemplate(Theme.Card, Theme.Accent, Theme.Radius);
            return b;
        }

        public static Button Primary(string text)
        {
            var b = Base(text, Theme.Accent, Brushes.White);
            b.Template = FlatTemplate(Theme.Accent, Theme.AccentHov, Theme.Radius);
            return b;
        }

        public static Button Secondary(string text)
        {
            var b = Base(text, Theme.Card, Theme.Text);
            b.Template = FlatTemplate(Theme.Card, Theme.Border, Theme.Radius);
            return b;
        }

        public static Button Danger(string text)
        {
            var b = Base(text, Theme.Danger, Brushes.White);
            b.Template = FlatTemplate(Theme.Danger, Theme.Danger, Theme.Radius);
            return b;
        }

        private static Button Base(string text, Brush bg, Brush fg) => new()
        {
            Content = text,
            Height = 38,
            MinWidth = 110,
            Padding = new Thickness(18, 0, 18, 0),
            Background = bg,
            Foreground = fg,
            BorderThickness = new Thickness(0),
            FontSize = 14,
            FontFamily = Theme.Font,
            FontWeight = FontWeights.SemiBold,
            Cursor = Cursors.Hand
        };

        private static ControlTemplate FlatTemplate(Brush normal, Brush hover, CornerRadius radius)
        {
            var border = new FrameworkElementFactory(typeof(Border));
            border.Name = "border";
            border.SetValue(Border.CornerRadiusProperty, radius);
            border.SetValue(Border.BackgroundProperty, normal);
            border.SetValue(Border.PaddingProperty, new Thickness(12, 4, 12, 4));

            var presenter = new FrameworkElementFactory(typeof(ContentPresenter));
            presenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            presenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            border.AppendChild(presenter);

            var tpl = new ControlTemplate(typeof(Button)) { VisualTree = border };
            var trigger = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
            trigger.Setters.Add(new Setter(Border.BackgroundProperty, hover, "border"));
            tpl.Triggers.Add(trigger);
            return tpl;
        }
    }
}