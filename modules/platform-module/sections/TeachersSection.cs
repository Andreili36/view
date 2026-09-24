using System.Windows;
using System.Windows.Controls;
using View.Modules.PlatformModule;
using View.Modules.PlatformModule.Models;
using View.Modules.PlatformModule.Sections;
using View.Ui;

namespace View.Modules.PlatformModule.Sections
{
    public class TeachersSection : SectionBase
    {
        public override string Title => "Преподаватели";
        public override bool IsVisible => RoleId == 1;

        public TeachersSection(PlatformMain host) : base(host) { }

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
            List<Teacher> data;
            try { data = LoadData(); }
            catch (Exception ex) { Host.ShowError(ex.Message); data = new(); }

            table.ItemsSource = data;
            Grid.SetRow(table, 1);
            root.Children.Add(table);

            return root;
        }

        private List<Teacher> LoadData()
        {
            return Db.SqlQuery(
                @"SELECT id, last_name, first_name, middle_name, birth_date,
                     gender, phone, email, hire_date, specialization
              FROM teacher ORDER BY last_name",
                r => new Teacher
                {
                    Id = r.GetInt32(0),
                    LastName = r.GetString(1),
                    FirstName = r.GetString(2),
                    MiddleName = r.IsDBNull(3) ? "" : r.GetString(3),
                    BirthDate = r.GetDateTime(4),
                    Gender = r.IsDBNull(5) ? "" : r.GetString(5),
                    Phone = r.IsDBNull(6) ? "" : r.GetString(6),
                    Email = r.IsDBNull(7) ? "" : r.GetString(7),
                    HireDate = r.GetDateTime(8),
                    Specialization = r.IsDBNull(9) ? "" : r.GetString(9)
                });
        }

        private void OnAdd()
        {
            var s = new Teacher();
            if (!TryEditDialog("Новый преподаватель", s)) return;

            try
            {
                Db.SqlExecute(
                    @"INSERT INTO teacher (last_name, first_name, middle_name, birth_date,
                                       gender, phone, email, hire_date, specialization)
                  VALUES (@ln, @fn, @mn, @bd, @g, @ph, @em, @hd, @sp)",
                    new()
                    {
                        ["@ln"] = s.LastName,
                        ["@fn"] = s.FirstName,
                        ["@mn"] = s.MiddleName,
                        ["@bd"] = s.BirthDate,
                        ["@g"] = s.Gender,
                        ["@ph"] = s.Phone,
                        ["@em"] = s.Email,
                        ["@hd"] = s.HireDate,
                        ["@sp"] = s.Specialization
                    });
                Host.ShowMessage("Преподаватель добавлен.");
                Host.RefreshCurrent();
            }
            catch (Exception ex) { Host.ShowError(ex.Message); }
        }

        private void OnEdit(FrameworkElement root)
        {
            var table = (DataGrid)((Grid)root).Children[1];
            if (table.SelectedItem is not Teacher s) { Host.ShowError("Выберите преподавателя."); return; }
            if (!TryEditDialog("Редактирование преподавателя", s)) return;

            try
            {
                Db.SqlExecute(
                    @"UPDATE teacher SET last_name=@ln, first_name=@fn, middle_name=@mn,
                         specialization=@sp, birth_date=@bd, gender=@g, phone=@ph, email=@em
                  WHERE id=@id",
                    new()
                    {
                        ["@ln"] = s.LastName,
                        ["@fn"] = s.FirstName,
                        ["@mn"] = s.MiddleName,
                        ["@sp"] = s.Specialization,
                        ["@bd"] = s.BirthDate,
                        ["@g"] = s.Gender,
                        ["@ph"] = s.Phone,
                        ["@em"] = s.Email,
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
            if (table.SelectedItem is not Teacher s) { Host.ShowError("Выберите преподавателя."); return; }
            if (!Host.Confirm($"Удалить преподавателя «{s.FullName}»?")) return;

            try
            {
                Db.SqlExecute("DELETE FROM teacher WHERE id=@id", new() { ["@id"] = s.Id });
                Host.ShowMessage("Удалено.");
                Host.RefreshCurrent();
            }
            catch (Exception ex) { Host.ShowError(ex.Message); }
        }

        private bool TryEditDialog(string title, Teacher s)
        {
            var fields = new List<FormField>
        {
            new() { Key = "last",   Label = "Фамилия",     Value = s.LastName },
            new() { Key = "first",  Label = "Имя",         Value = s.FirstName },
            new() { Key = "middle", Label = "Отчество",    Value = s.MiddleName },
            new() { Key = "specialization", Label = "Специализация",    Value = s.Specialization },
            new() { Key = "birth",  Label = "Дата рождения", Value = s.BirthDate.ToString("yyyy-MM-dd"), Kind = FieldKind.Date },
            new() { Key = "gender", Label = "Пол",         Value = s.Gender },
            new() { Key = "phone",  Label = "Телефон",     Value = s.Phone },
            new() { Key = "email",  Label = "Email",       Value = s.Email },
        };

            if (!CDPOFormDialog.Show(Host.Window, title, fields, out var result)) return false;

            try
            {
                s.LastName = result["last"];
                s.FirstName = result["first"];
                s.MiddleName = result["middle"];
                s.Specialization = result["specialization"];
                s.BirthDate = DateTime.ParseExact(result["birth"], "yyyy-MM-dd", null);
                s.Gender = result["gender"];
                s.Phone = result["phone"];
                s.Email = result["email"];
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