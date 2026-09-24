using System.Windows;
using System.Windows.Controls;
using View.Modules.PlatformModule.Sections;

namespace View.Modules.PlatformModule;

public class PlatformMain
{
    private readonly MainForm _form;
    private readonly List<SectionBase> _sections;
    private readonly ContentControl _contentHost = new();
    private SectionBase? _current;

    public int RoleId { get; }
    public string UserName { get; }
    public Window Window => _form;
    public Data.DbWrapper Db => _form.Db;

    public PlatformMain(MainForm form, int roleId, string userName)
    {
        _form = form;
        RoleId = roleId;
        UserName = userName;

        _sections = new List<SectionBase>
        {
            new StudentsSection(this),
            new TeachersSection(this),
            new ProgramsSection(this),
            new CoursesSection(this)
        };
    }

    public FrameworkElement Build()
    {
        var grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(240) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        var menuBorder = new Border
        {
            Background = Ui.Theme.Surface,
            Padding = new Thickness(16, 24, 16, 24)
        };
        menuBorder.Child = new PlatformMenu(this, _sections).Build();
        Grid.SetColumn(menuBorder, 0);
        grid.Children.Add(menuBorder);

        var contentBorder = new Border
        {
            Background = Ui.Theme.Bg,
            Padding = new Thickness(24)
        };
        contentBorder.Child = _contentHost;
        Grid.SetColumn(contentBorder, 1);
        grid.Children.Add(contentBorder);

        var first = _sections.FirstOrDefault(s => s.IsVisible);
        if (first != null) NavigateTo(first);

        return grid;
    }

    public void NavigateTo(SectionBase section)
    {
        _current = section;
        _contentHost.Content = section.Build();
    }

    public void RefreshCurrent()
    {
        if (_current != null)
            _contentHost.Content = _current.Build();
    }

    public void ShowMessage(string text) => _form.ShowMessage(text);
    public void ShowError(string text) => _form.ShowError(text);
    public bool Confirm(string text) => _form.Confirm(text);
}