using NUnit.Framework;
using System;
using View.Modules.AuthModule;
using View.Tests;

namespace View.Tests
{
    [TestFixture]
    public class AuthServiceTest
    {
        private FakeDbWrapper _fakeDb = null!;
        private AuthService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _fakeDb = new FakeDbWrapper();
            _service = new AuthService(_fakeDb);
        }

        [Test]
        [Description("Пустой логин — доступ запрещён")]
        public void Authenticate_EmptyLogin_ReturnsZeroRole()
        {
            var (roleId, userName) = _service.Authenticate("", "111");

            Assert.That(roleId, Is.EqualTo(null));
            Assert.That(userName, Is.Empty);
        }

        [Test]
        [Description("Пустой пароль — доступ запрещён")]
        public void Authenticate_EmptyPassword_ReturnsZeroRole()
        {
            var (roleId, userName) = _service.Authenticate("admin", "");

            Assert.That(roleId, Is.EqualTo(null));
            Assert.That(userName, Is.Empty);
        }

        [Test]
        [Description("Пробелы вместо логина и пароля — доступ запрещён")]
        public void Authenticate_WhitespaceInput_ReturnsZeroRole()
        {
            var (roleId, _) = _service.Authenticate("   ", "   ");

            Assert.That(roleId, Is.EqualTo(null));
        }

        [Test]
        [Description("Несуществующий пользователь — доступ запрещён")]
        public void Authenticate_UnknownUser_ReturnsZeroRole()
        {
            _fakeDb.SetupScalar("SELECT role_id", null);

            var (roleId, userName) = _service.Authenticate("unknown", "111");

            Assert.That(roleId, Is.EqualTo(null));
            Assert.That(userName, Is.Empty);
        }

        [Test]
        [Description("Успешная авторизация администратора — роль 1")]
        public void Authenticate_Admin_Success()
        {
            _fakeDb.SetupScalar("SELECT role_id", 1);
            _fakeDb.SetupScalar("SELECT last_name", "Иванов Иван");

            var (roleId, userName) = _service.Authenticate("admin", "111");

            Assert.That(roleId, Is.EqualTo(1));
            Assert.That(userName, Is.EqualTo("Иванов Иван"));
        }

        [Test]
        [Description("Успешная авторизация пользователя — роль 2")]
        public void Authenticate_User_Success()
        {
            _fakeDb.SetupScalar("SELECT role_id", 2);
            _fakeDb.SetupScalar("SELECT last_name", "Петров Пётр");

            var (roleId, userName) = _service.Authenticate("user", "222");

            Assert.That(roleId, Is.EqualTo(2));
            Assert.That(userName, Is.EqualTo("Петров Пётр"));
        }

        [Test]
        [Description("Успешная авторизация гостя — роль 3")]
        public void Authenticate_Guest_Success()
        {
            _fakeDb.SetupScalar("SELECT role_id", 3);
            _fakeDb.SetupScalar("SELECT last_name", "Гостев Гость");

            var (roleId, userName) = _service.Authenticate("guest", "333");

            Assert.That(roleId, Is.EqualTo(3));
            Assert.That(userName, Is.EqualTo("Гостев Гость"));
        }

        [Test]
        [Description("Ошибка подключения к БД пробрасывается наружу")]
        public void Authenticate_DbError_ThrowsException()
        {
            _fakeDb.SetupException(new InvalidOperationException("Нет соединения с БД"));

            Assert.Throws<InvalidOperationException>(
                () => _service.Authenticate("admin", "111"));
        }

        [Test]
        [Description("Возвращаемая роль — целое число, а не строка")]
        public void Authenticate_ReturnsIntegerRole()
        {
            _fakeDb.SetupScalar("SELECT role_id", "1");
            _fakeDb.SetupScalar("SELECT last_name", "Иванов Иван");

            var (roleId, _) = _service.Authenticate("admin", "111");

            Assert.That(roleId, Is.EqualTo(1));
            Assert.That(roleId, Is.TypeOf<int>());
        }
    }
}