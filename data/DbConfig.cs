using System.IO;
using System.Text.Json;

namespace View.Data;

public class DbConfig
{
    public required string Host { get; set; }
    public required int Port { get; set; }
    public required string Name { get; set; }
    public required string User { get; set; }
    public required string Password { get; set; }

    public string ToConnectionString() =>
        $"Host={Host};Port={Port};Database={Name};Username={User};Password={Password};";

    public static DbConfig Load()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "app-config.json");
        if (!File.Exists(path))
            throw new FileNotFoundException("Не найден app-config.json рядом с .exe", path);

        using var doc = JsonDocument.Parse(File.ReadAllText(path));
        var d = doc.RootElement.GetProperty("database");

        return new DbConfig
        {
            Host = d.GetProperty("host").GetString() ?? "",
            Port = d.GetProperty("port").GetInt32(),
            Name = d.GetProperty("name").GetString() ?? "",
            User = d.GetProperty("user").GetString() ?? "",
            Password = d.GetProperty("password").GetString() ?? ""
        };
    }
}