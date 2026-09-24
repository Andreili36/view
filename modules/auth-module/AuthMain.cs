using System.Windows;
using System.Windows.Controls;

namespace View.Modules.AuthModule
{
	public class AuthMain
	{
		private readonly MainForm _host;
		private readonly AuthService _service;

		private readonly TextBox _login = Ui.CDPOInput.Text();
		private readonly PasswordBox _password = Ui.CDPOInput.Password();
		private readonly TextBlock _error = new()
		{
			Foreground = Ui.Theme.Danger,
			FontSize = 12,
			Margin = new Thickness(0, 8, 0, 0),
			Visibility = Visibility.Collapsed
		};

		public event Action<int, string>? OnAuthorized;

		public AuthMain(MainForm host)
		{
			_host = host;
			_service = new AuthService(host.Db);
		}

		public FrameworkElement Build()
		{
			var panel = new StackPanel
			{
				Margin = new Thickness(40, 60, 40, 40),
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Width = 360
			};

			panel.Children.Add(Ui.CDPOText.H1("Вход в систему"));
			panel.Children.Add(Ui.CDPOText.Dim("Центр дополнительного профессионального образования"));
			panel.Children.Add(Ui.CDPOCard.Spacer(24));

			panel.Children.Add(Ui.CDPOText.Label("Логин"));
			panel.Children.Add(_login);
			panel.Children.Add(Ui.CDPOCard.Spacer(12));

			panel.Children.Add(Ui.CDPOText.Label("Пароль"));
			panel.Children.Add(_password);

			panel.Children.Add(_error);
			panel.Children.Add(Ui.CDPOCard.Spacer(24));

			var buttons = new Grid();
			buttons.ColumnDefinitions.Add(new ColumnDefinition());
			buttons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(12) });
			buttons.ColumnDefinitions.Add(new ColumnDefinition());

			var cancel = Ui.CDPOButton.Secondary("Отмена");
			cancel.Click += (_, _) => Application.Current.Shutdown();
			Grid.SetColumn(cancel, 0);

			var ok = Ui.CDPOButton.Primary("Войти");
			ok.Click += (_, _) => DoLogin();
			Grid.SetColumn(ok, 2);

			buttons.Children.Add(cancel);
			buttons.Children.Add(ok);
			panel.Children.Add(buttons);

			_password.KeyDown += (_, e) =>
			{
				if (e.Key == System.Windows.Input.Key.Enter) DoLogin();
			};

			var root = new Grid();
			root.Children.Add(panel);
			return root;
		}

		private void DoLogin()
		{
			_error.Visibility = Visibility.Collapsed;

			string login = _login.Text.Trim();
			string pass = _password.Password;

			if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(pass))
			{
				ShowError("Заполните все поля.");
				return;
			}

			try
			{
				var (roleId, userName) = _service.Authenticate(login, pass);
				if (roleId == null)
				{
					ShowError("Неверный логин или пароль.");
					return;
				}
                if (roleId == 0)
                {
                    ShowError("Доступ запрещен.");
                    return;
                }
                OnAuthorized?.Invoke((int)roleId, userName);
			}
			catch (Exception ex)
			{
				ShowError("Нет подключения к БД: " + ex.Message);
			}
		}

		private void ShowError(string text)
		{
			_error.Text = text;
			_error.Visibility = Visibility.Visible;
		}
	}
}