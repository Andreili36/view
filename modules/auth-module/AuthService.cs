using View.Data;

namespace View.Modules.AuthModule
{
    public class AuthService
    {
        private readonly DbWrapper _db;
        public AuthService(DbWrapper db) => _db = db;

        public (int roleId, string userName) Authenticate(string login, string password)
        {
            var role = _db.SqlScalar(
                "SELECT role_id FROM app_user WHERE login = @l AND password = @p",
                new() { ["@l"] = login, ["@p"] = password });

            if (role == null) return (0, "");

            var name = _db.SqlScalar(
                "SELECT last_name || ' ' || first_name FROM app_user WHERE login = @l",
                new() { ["@l"] = login })?.ToString() ?? login;

            return (Convert.ToInt32(role), name);
        }
    }
}