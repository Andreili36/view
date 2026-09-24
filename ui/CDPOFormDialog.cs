using System.Windows;
using System.Windows.Controls;

namespace View.Ui
{
    public enum FieldKind { Text, Date, Int }

    public class FormField
    {
        public string Key { get; set; } = "";
        public string Label { get; set; } = "";
        public string Value { get; set; } = "";
        public FieldKind Kind { get; set; } = FieldKind.Text;
    }

    public static class CDPOFormDialog
    {
        public static bool Show(Window owner, string title, List<FormField> fields, out Dictionary<string, string> result)
        {
            var localResult = new Dictionary<string, string>();

            result = new Dictionary<string, string>();

            var win = new Window
            {
                Title = title,
                Width = 460,
                SizeToContent = SizeToContent.Height,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = owner,
                Background = Theme.Bg,
                Foreground = Theme.Text,
                FontFamily = Theme.Font
            };

            var panel = new StackPanel { Margin = new Thickness(24) };
            panel.Children.Add(CDPOText.H1(title));

            var inputs = new Dictionary<string, TextBox>();

            foreach (var f in fields)
            {
                string label = f.Kind == FieldKind.Date ? f.Label + " (гггг-ММ-дд)" : f.Label;
                panel.Children.Add(CDPOText.Label(label));
                var tb = CDPOInput.Text(f.Value);
                inputs[f.Key] = tb;
                panel.Children.Add(tb);
                panel.Children.Add(CDPOCard.Spacer(10));
            }

            var buttons = new Grid();
            buttons.ColumnDefinitions.Add(new ColumnDefinition());
            buttons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(12) });
            buttons.ColumnDefinitions.Add(new ColumnDefinition());

            var cancel = CDPOButton.Secondary("Отмена");
            cancel.Click += (_, _) => win.DialogResult = false;

            var ok = CDPOButton.Primary("Сохранить");
            ok.Click += (_, _) =>
            {
                localResult.Clear();
                foreach (var (k, tb) in inputs)
                    localResult[k] = tb.Text.Trim();
                win.DialogResult = true;
            };

            Grid.SetColumn(cancel, 0);
            Grid.SetColumn(ok, 2);
            buttons.Children.Add(cancel);
            buttons.Children.Add(ok);
            panel.Children.Add(buttons);

            win.Content = panel;
            var resultShowDialog = win.ShowDialog();

            result = localResult;

            return resultShowDialog == true;
        }
    }
}