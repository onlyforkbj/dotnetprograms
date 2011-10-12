using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace DbTool.Lib.Communication.Commands
{
    public class QueryExecutor : IDbCommandExecutor
    {
        private readonly DbConnection _dbConnection;
        private readonly int _maxRows;

        public QueryExecutor(DbConnection dbConnection, int maxRows)
        {
            _dbConnection = dbConnection;
            _maxRows = maxRows;
        }

        public IDbCommandResult Execute(string query)
        {
            using (var command = _dbConnection.CreateCommand())
            {
                try
                {
                    command.CommandText = query.Trim();
                    command.Connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        var result = CreateResult(reader);
                        while (reader.Read())
                        {
                            var values = new List<object>();
                            for (var ii = 0; ii < reader.FieldCount; ii++)
                            {
                                values.Add(reader.GetValue(ii));
                            }
                            result.AddRow(values);
                            if (_maxRows > 0 && result.Rowcount >= _maxRows)
                            {
                                break;
                            }
                        }
                        return result;
                    }
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }

        private static QueryResult CreateResult(IDataRecord reader)
        {
            var result = new QueryResult();
            for (var ii = 0; ii < reader.FieldCount; ii++)
            {
                var name = reader.GetName(ii);
                var type = reader.GetFieldType(ii);
                result.AddColumn(name, type);
            }
            return result;
        }
    }
}