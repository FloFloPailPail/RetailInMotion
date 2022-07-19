using RetailInMotionContracts.DAL;
using RetailInMotionObjects.Models;
using System;
using System.Collections.Generic;

namespace RetailInMotionDAL.Repositories
{
    public class OrderItemsRepository : RepositoryBase, IOrderItemsRepository
    {
        public OrderItemsRepository(string connectionString) : base(connectionString)
        {
        }

        public int Add(List<OrderItem> orderItems)
        {
            string sql =
                @"INSERT INTO OrderItems 
                (OrderId, Quantity, ProductGuid)
                    Values 
                (@OrderId, @Quantity, @ProductGuid);
                SELECT CAST(SCOPE_IDENTITY() as int)";

            var result = 0;
            using (var connection = GetConnection())
            {
                result = connection.Execute(sql, orderItems);
            }

            return result;
        }

        public int Delete(List<int> orderItemIds)
        {
            var query = "" +
                @"DELETE FROM
                    OrderItems 
                WHERE
                    Id IN @orderItemIds
                ";

            var result = 0;
            using (var connection = GetConnection())
            {
                result = connection.Execute(query, new { orderItemIds = orderItemIds });
            }

            return result;
        }

        public int Edit(List<OrderItem> orderItems)
        {
            string query = @"
                    UPDATE
                        OrderItems
                    SET
                        OrderId = @OrderId,
                        ProductGuid = @ProductGuid,
                        Quantity = @Quantity
                    WHERE
                        Id = @Id;
                    ";

            var result = 0;
            using (var connection = GetConnection())
            {
                result = connection.Execute(query, orderItems);
            }

            return result;
        }
    }
}
