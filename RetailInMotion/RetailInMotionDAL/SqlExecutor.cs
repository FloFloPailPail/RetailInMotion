using Dapper;
using RetailInMotionContracts.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace RetailInMotionDAL
{
    public class SqlExecutor : IDbExecutor
    {
        readonly SqlConnection _sqlConnection;

        public SqlExecutor(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public int Execute(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return _sqlConnection.Execute(sql, param, transaction, commandTimeout, commandType);
        }

        public IEnumerable<dynamic> Query(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null,
            CommandType? commandType = null
            )
        {
            return _sqlConnection.Query(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return _sqlConnection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(
            string sql,
            Func<TFirst, TSecond, TReturn> map,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return _sqlConnection.Query<TFirst, TSecond, TReturn>(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public void OpenConection()
        {
            _sqlConnection.Open();
        }

        public void CloseConection()
        {
            _sqlConnection.Close();
        }

        public void Dispose()
        {
            _sqlConnection.Dispose();
        }
    }
}
