using System;
using System.Collections.Generic;
using System.Data;

namespace RetailInMotionContracts.DAL
{
    public interface IDbExecutor : IDisposable
    {
        public int Execute(
               string sql,
               object param = null,
               IDbTransaction transaction = null,
               int? commandTimeout = null,
               CommandType? commandType = null);

        public IEnumerable<T> Query<T>(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null,
            CommandType? commandType = null);

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(
            string sql,
            Func<TFirst, TSecond, TReturn> map,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null);

        public void OpenConection();

        public void CloseConection();
    }
}
