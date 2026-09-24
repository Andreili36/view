using System.Windows;
using System.Windows.Controls;
using View.Modules.MainModule.Models;
using View.Modules.PlatformModule;
using View.Modules.PlatformModule.Models;
using View.Modules.PlatformModule.Sections;
using View.Ui;

namespace View.Modules.PlatformModule.Sections
{
    public class ProgramsSection : SectionBase
    {
        public override string Title => "Программы обучения";
        public override bool IsVisible => RoleId == 1;

        public ProgramsSection(PlatformMain host) : base(host) { }

        public override FrameworkElement Build()
        {
            var root = new Grid();
            root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var header = new Grid();
            header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            header.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            header.Children.Add(CDPOText.H1(Title));

            var actions = new StackPanel { Orientation = Orientation.Horizontal };

            if (CanCreate)
            {
                var add = CDPOButton.Primary("Добавить");
                add.Click += (_, _) => OnAdd();
                actions.Children.Add(add);
                actions.Children.Add(CDPOCard.Spacer(0));
                actions.Children.Add(new Border { Width = 8 });
            }

            if (CanEdit)
            {
                var edit = CDPOButton.Secondary("Изменить");
                edit.Click += (_, _) => OnEdit(root);
                actions.Children.Add(edit);
                actions.Children.Add(new Border { Width = 8 });
            }

            if (CanDelete)
            {
                var del = CDPOButton.Danger("Удалить");
                del.Click += (_, _) => OnDelete(root);
                actions.Children.Add(del);
                actions.Children.Add(new Border { Width = 8 });
            }

            var refresh = CDPOButton.Secondary("Обновить");
            refresh.Click += (_, _) => Host.RefreshCurrent();
            actions.Children.Add(refresh);

            Grid.SetColumn(actions, 1);
            header.Children.Add(actions);
            Grid.SetRow(header, 0);
            root.Children.Add(header);

            var table = CDPOTable.Table();
            table.Name = "Grid";
            List<ProgramItem> data;
            try { data = LoadData(); }
            catch (Exception ex) { Host.ShowError(ex.Message); data = new(); }

            table.ItemsSource = data;
            Grid.SetRow(table, 1);
            root.Children.Add(table);

            return root;
        }

        private List<ProgramItem> LoadData()
        {
            return Db.SqlQuery(
                @"SELECT id, name, description, duration_hours, category
              FROM program ORDER BY name",
                r => new ProgramItem
                {
                    Id = r.GetInt32(0),
                    Name = r.GetString(1),
                    Description = r.IsDBNull(2) ? "" : r.GetString(2),
                    DurationHours = r.IsDBNull(3) ? 0 : r.GetInt32(3),
                    Category = r.IsDBNull(4) ? "" : r.GetString(4)
                });
        }

        private void OnAdd()
        {
            var s = new ProgramItem();
            if (!TryEditDialog("Новая программа обучения", s)) return;

            try
            {
                Db.SqlExecute(
                    @"INSERT INTO program (name, description, duration_hours, category)
                  VALUES (@n,@ds, @dh, @c)",
                    new()
                    {
                        ["@n"] = s.Name,
                        ["@ds"] = s.Description,
                        ["@dh"] = s.DurationHours,
                        ["@c"] = s.Category
                    });
                Host.ShowMessage("Программа обучения добавлена.");
                Host.RefreshCurrent();
            }
            catch (Exception ex) { Host.ShowError(ex.Message); }
        }

        private void OnEdit(FrameworkElement root)
        {
            var table = (DataGrid)((Grid)root).Children[1];
            if (table.SelectedItem is not ProgramItem s) { Host.ShowError("Выберите программу обучения."); return; }
            if (!TryEditDialog("Редактирование программы обучения", s)) return;

            try
            {
                Db.SqlExecute(
                    @"UPDATE program SET name=@n, description=@ds, duration_hours=@dh,
                         category=@c
                  WHERE id=@id",
                    new()
                    {
                        ["@n"] = s.Name,
                        ["@ds"] = s.Description,
                        ["@dh"] = s.DurationHours,
                        ["@c"] = s.Category,
                        ["@id"] = s.Id
                    });
                Host.ShowMessage("Сохранено.");
                Host.RefreshCurrent();
            }
            catch (Exception ex) { Host.ShowError(ex.Message); }
        }

        private void OnDelete(FrameworkElement root)
        {
            var table = (DataGrid)((Grid)root).Children[1];
            if (table.SelectedItem is not ProgramItem s) { Host.ShowError("Выберите программу обучения."); return; }
            if (!Host.Confirm($"Удалить программу обучения «{s.Name}»?")) return;

            try
            {
                Db.SqlExecute("DELETE FROM program WHERE id=@id", new() { ["@id"] = s.Id });
                Host.ShowMessage("Удалено.");
                Host.RefreshCurrent();
            }
            catch (Exception ex) { Host.ShowError(ex.Message); }
        }

        private bool TryEditDialog(string title, ProgramItem s)
        {
            var fields = new List<FormField>
        {
            new() { Key = "name",   Label = "Название",     Value = s.Name },
            new() { Key = "description",  Label = "Описание",         Value = s.Description },
            new() { Key = "duration_hourse", Label = "Время продолжения",    Value = s.DurationHours.ToString() },
            new() { Key = "category",  Label = "Категория", Value = s.Category }
        };

            if (!CDPOFormDialog.Show(Host.Window, title, fields, out var result)) return false;

            try
            {
                s.Name = result["name"];
                s.Description = result["description"];
                s.DurationHours = Convert.ToInt32( result["duration_hourse"] );
                s.Category = result["category"];
                return true;
            }
            catch (Exception ex)
            {
                Host.ShowError("Ошибка в данных: " + ex.Message);
                return false;
            }
        }
    }
}