using RetailInMotionContracts.DAL;
using System.Data.SqlClient;

namespace RetailInMotionDAL.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionString;

        public RepositoryBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected IDbExecutor GetConnection()
        {
            return new SqlExecutor(new SqlConnection(_connectionString));
        }
    }
}
