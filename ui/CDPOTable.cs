using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace View.Ui
{
    public static class CDPOTable
    {
        public static DataGrid Table() => new()
        {
            AutoGenerateColumns = true,
            CanUserAddRows = false,
            CanUserDeleteRows = false,
            IsReadOnly = true,
            HeadersVisibility = DataGridHeadersVisibility.Column,
            GridLinesVisibility = DataGridGridLinesVisibility.Horizontal,
            Background = Theme.Card,
            RowBackground = Theme.Card,
            AlternatingRowBackground = Theme.Surface,
            Foreground = Theme.Text,
            BorderBrush = Theme.Border,
            BorderThickness = new Thickness(1),
            FontFamily = Theme.Font,
            FontSize = 13,
            RowHeight = 34,
            HorizontalGridLinesBrush = Theme.Border,
            SelectionMode = DataGridSelectionMode.Single,
            ColumnHeaderStyle = ColumnHeaderStyle(),
            RowHeaderStyle = RowHeaderStyle(),
            ColumnHeaderHeight = 40
        };

        private static Style ColumnHeaderStyle()
        {
            var style = new Style(typeof(DataGridColumnHeader));

            style.Setters.Add(new Setter(Control.ForegroundProperty, Theme.Text));
            style.Setters.Add(new Setter(Control.FontWeightProperty, FontWeights.SemiBold));
            style.Setters.Add(new Setter(Control.FontSizeProperty, 13d));
            style.Setters.Add(new Setter(Control.FontFamilyProperty, Theme.Font));
            style.Setters.Add(new Setter(Control.PaddingProperty, new Thickness(12, 0, 12, 0)));
            style.Setters.Add(new Setter(Control.VerticalContentAlignmentProperty, VerticalAlignment.Center));
            style.Setters.Add(new Setter(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Left));

            var border = new FrameworkElementFactory(typeof(Border));
            border.Name = "border";
            border.SetValue(Border.BackgroundProperty, Theme.Surface);
            border.SetValue(Border.BorderBrushProperty, Theme.Border);
            border.SetValue(Border.BorderThicknessProperty, new Thickness(0, 0, 1, 1));
            border.SetValue(Border.PaddingProperty, new Thickness(12, 0, 12, 0));

            var presenter = new FrameworkElementFactory(typeof(ContentPresenter));
            presenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            presenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            border.AppendChild(presenter);

            var template = new ControlTemplate(typeof(DataGridColumnHeader)) { VisualTree = border };

            var hover = new Trigger
            {
                Property = DataGridColumnHeader.IsMouseOverProperty,
                Value = true
            };
            hover.Setters.Add(new Setter(Border.BackgroundProperty, Theme.Card, "border"));
            template.Triggers.Add(hover);

            style.Setters.Add(new Setter(Control.TemplateProperty, template));
            return style;
        }

        private static Style RowHeaderStyle()
        {
            var style = new Style(typeof(DataGridRowHeader));
            style.Setters.Add(new Setter(Control.BackgroundProperty, Theme.Surface));
            style.Setters.Add(new Setter(Control.ForegroundProperty, Theme.Text));
            style.Setters.Add(new Setter(Control.BorderBrushProperty, Theme.Border));
            return style;
        }
    }
}