using RetailInMotionContracts.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RetailInMotionDAL.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionString;

        public RepositoryBase(string connectionString/*, ICache cache*/)
        {
            _connectionString = connectionString;
        }

        protected IDbExecutor GetConnection()
        {
            return new SqlExecutor(new SqlConnection(_connectionString));
        }
    }
}
