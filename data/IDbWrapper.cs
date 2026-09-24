using System.Data;

namespace View.Data
{
    public interface IDbWrapper
    {
        void TestConnection();

        List<T> SqlQuery<T>(
            string sql,
            Func<IDataRecord, T> mapper,
            Dictionary<string, object?>? parameters = null);

        object? SqlScalar(
            string sql,
            Dictionary<string, object?>? parameters = null);

        int SqlExecute(
            string sql,
            Dictionary<string, object?>? parameters = null);
    }
}