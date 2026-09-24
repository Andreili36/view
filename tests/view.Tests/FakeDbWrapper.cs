using System;
using System.Collections.Generic;
using System.Data;
using View.Data;

namespace View.Tests
{
    public class FakeDbWrapper : IDbWrapper
    {
        private readonly Dictionary<string, object?> _scalarResponses = new();
        private Exception? _exceptionToThrow;

        public void TestConnection()
        {
            if (_exceptionToThrow != null) throw _exceptionToThrow;
        }

        public void SetupScalar(string sqlContains, object? response)
        {
            _scalarResponses[sqlContains] = response;
        }

        public void SetupException(Exception ex)
        {
            _exceptionToThrow = ex;
        }

        public object? SqlScalar(string sql, Dictionary<string, object?>? parameters = null)
        {
            if (_exceptionToThrow != null) throw _exceptionToThrow;

            foreach (var (key, value) in _scalarResponses)
            {
                if (sql.Contains(key, StringComparison.OrdinalIgnoreCase))
                    return value;
            }
            return null;
        }

        public List<T> SqlQuery<T>(
            string sql,
            Func<IDataRecord, T> mapper,
            Dictionary<string, object?>? parameters = null)
        {
            if (_exceptionToThrow != null) throw _exceptionToThrow;
            return new List<T>();
        }

        public int SqlExecute(
            string sql,
            Dictionary<string, object?>? parameters = null)
        {
            if (_exceptionToThrow != null) throw _exceptionToThrow;
            return 0;
        }
    }
}