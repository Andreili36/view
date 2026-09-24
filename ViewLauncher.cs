using System.Windows;
using View.Data;

namespace View
{
    public static class ViewLauncher
    {
        public static void Run()
        {
            IDbWrapper db;
            try
            {
                db = new DbWrapper();
                db.TestConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось подключиться к базе данных.\n\n" + ex.Message,
                    "Ошибка подключения",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            var app = new Application { ShutdownMode = ShutdownMode.OnMainWindowClose };
            var main = new MainForm(db);
            app.MainWindow = main;
            app.Run(main);
        }
    }
}