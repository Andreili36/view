using System.Windows;
using System.Windows.Controls;
using View.Modules.PlatformModule.Models;
using View.Modules.PlatformModule.Sections;
using View.Ui;

namespace View.Modules.PlatformModule.Sections
{
    public class CoursesSection : SectionBase
    {
        public override string Title => "Курсы";
        public override bool IsVisible => true;
        public override bool CanCreate => RoleId == 1;
        public override bool CanEdit => RoleId == 1 || RoleId == 2;
        public override bool CanDelete => RoleId == 1;

        public CoursesSection(PlatformMain host) : base(host) { }

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
            List<CourseItem> data;
            try { data = LoadData(); }
            catch (Exception ex) { Host.ShowError(ex.Message); data = new(); }
            table.ItemsSource = data;
            Grid.SetRow(table, 1);
            root.Children.Add(table);

            return root;
        }

        private List<CourseItem> LoadData() =>
            Db.SqlQuery(
                @"SELECT c.id, c.program_id, p.name, c.start_date, c.end_date,
                     c.teacher_id,
                     COALESCE(t.last_name || ' ' || t.first_name, '') AS tn,
                     c.max_students,
                     (SELECT COUNT(*) FROM enrollment e WHERE e.course_id = c.id)
              FROM course c
              JOIN program p ON p.id = c.program_id
              LEFT JOIN teacher t ON t.id = c.teacher_id
              ORDER BY c.start_date DESC",
                r => new CourseItem
                {
                    Id = r.GetInt32(0),
                    ProgramId = r.GetInt32(1),
                    ProgramName = r.GetString(2),
                    StartDate = r.GetDateTime(3),
                    EndDate = r.GetDateTime(4),
                    TeacherId = r.IsDBNull(5) ? null : r.GetInt32(5),
                    TeacherName = r.GetString(6),
                    MaxStudents = r.GetInt32(7),
                    Enrolled = r.GetInt32(8)
                });

        private void OnAdd()
        {
            var c = new CourseItem();
            if (!TryEditDialog("Новый курс", c)) return;
            try
            {
                Db.SqlExecute(
                    @"INSERT INTO course (program_id, start_date, end_date, teacher_id, max_students)
                  VALUES (@pid, @sd, @ed, @tid, @ms)",
                    new()
                    {
                        ["@pid"] = c.ProgramId,
                        ["@sd"] = c.StartDate,
                        ["@ed"] = c.EndDate,
                        ["@tid"] = c.TeacherId,
                        ["@ms"] = c.MaxStudents
                    });
                Host.ShowMessage("Курс добавлен.");
                Host.RefreshCurrent();
            }
            catch (Exception ex) { Host.ShowError(ex.Message); }
        }

        private void OnEdit(FrameworkElement root)
        {
            var table = (DataGrid)((Grid)root).Children[1];
            if (table.SelectedItem is not CourseItem c) { Host.ShowError("Выберите курс."); return; }
            if (!TryEditDialog("Редактирование курса", c)) return;
            try
            {
                Db.SqlExecute(
                    @"UPDATE course SET program_id=@pid, start_date=@sd, end_date=@ed,
                         teacher_id=@tid, max_students=@ms
                  WHERE id=@id",
                    new()
                    {
                        ["@pid"] = c.ProgramId,
                        ["@sd"] = c.StartDate,
                        ["@ed"] = c.EndDate,
                        ["@tid"] = c.TeacherId,
                        ["@ms"] = c.MaxStudents,
                        ["@id"] = c.Id
                    });
                Host.ShowMessage("Сохранено.");
                Host.RefreshCurrent();
            }
            catch (Exception ex) { Host.ShowError(ex.Message); }
        }

        private void OnDelete(FrameworkElement root)
        {
            var table = (DataGrid)((Grid)root).Children[1];
            if (table.SelectedItem is not CourseItem c) { Host.ShowError("Выберите курс."); return; }
            if (!Host.Confirm($"Удалить курс #{c.Id}?")) return;
            try
            {
                Db.SqlExecute("DELETE FROM course WHERE id=@id", new() { ["@id"] = c.Id });
                Host.ShowMessage("Удалено.");
                Host.RefreshCurrent();
            }
            catch (Exception ex) { Host.ShowError(ex.Message); }
        }

        private bool TryEditDialog(string title, CourseItem c)
        {
            var fields = new List<FormField>
        {
            new() { Key = "pid", Label = "ID программы", Value = c.ProgramId.ToString(), Kind = FieldKind.Int },
            new() { Key = "sd",  Label = "Дата начала",  Value = c.StartDate.ToString("yyyy-MM-dd"), Kind = FieldKind.Date },
            new() { Key = "ed",  Label = "Дата окончания", Value = c.EndDate.ToString("yyyy-MM-dd"), Kind = FieldKind.Date },
            new() { Key = "ms",  Label = "Максимум студентов", Value = c.MaxStudents.ToString(), Kind = FieldKind.Int },
        };
            if (!CDPOFormDialog.Show(Host.Window, title, fields, out var r)) return false;
            try
            {
                c.ProgramId = int.Parse(r["pid"]);
                c.StartDate = DateTime.ParseExact(r["sd"], "yyyy-MM-dd", null);
                c.EndDate = DateTime.ParseExact(r["ed"], "yyyy-MM-dd", null);
                c.MaxStudents = int.Parse(r["ms"]);
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