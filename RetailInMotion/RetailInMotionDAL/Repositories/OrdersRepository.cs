using Dapper;
using RetailInMotionContracts.DAL;
using RetailInMotionObjects;
using RetailInMotionObjects.DataTransferObjects;
using RetailInMotionObjects.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RetailInMotionDAL.Repositories
{
    public class OrdersRepository : RepositoryBase, IOrdersRepository
    {
        public OrdersRepository(string connectionString) : base(connectionString)
        {
        }

        public int Add()
        {
            var utcNow = DateTime.UtcNow;
            string sql =
                @"INSERT INTO Orders 
                (AddressLine1, AddressLine2, AddressLine3, AddressLine4, PostalCode, CountryCode, DateTimeCreated, DateTimeUpdated)
                Values
                (null, null, null, null, null, null, @utcNow, @utcNow);
                SELECT CAST(SCOPE_IDENTITY() as int)";

            var result = 0;
            using (var connection = GetConnection())
            {
                result = connection.Query<int>(sql, new { utcNow = utcNow }).FirstOrDefault();
            }

            return result;
        }

        public int Add(Order order)
        {
            string sql =
                @"INSERT INTO Orders 
                (AddressLine1, AddressLine2, AddressLine3, AddressLine4, PostalCode, CountryCode, DateTimeCreated, DateTimeUpdated) 
                Values 
                (@AddressLine1, @AddressLine2, @AddressLine3, @AddressLine4, @PostalCode, @CountryCode, @DateTimeCreated, @DateTimeUpdated);
                SELECT CAST(SCOPE_IDENTITY() as int)";

            var result = 0;
            using (var connection = GetConnection())
            {
                result = connection.Query<int>(sql, order).FirstOrDefault();
            }

            return result;
        }

        public int DeleteSoft(int id)
        {
            var utcNow = DateTime.UtcNow;
            string query = @"
                    UPDATE
                        Orders
                    SET 
                        DateTimeUpdated = @utcNow,
                        DateTimeDeleted = @utcNow
                    WHERE
                        Id = @Id;
                    ";

            var result = 0;
            using (var connection = GetConnection())
            {
                result = connection.Execute(query, new { utcNow = utcNow, Id = id });
            }

            return result;
        }

        public Order Get(int id)
        {
            string query =
                @"SELECT * FROM
                    Orders
                  WHERE
                    Id = @Id
                    AND DateTimeDeleted IS NULL
                ";
            Order order = null;
            using (var connection = GetConnection())
            {
                order = connection.Query<Order>(query, new { Id = id }).FirstOrDefault();
            }

            return order;
        }

        public OrderDTO GetWithItems(int id)
        {
            string query = @"
                    SELECT * FROM
                        Orders o
                    LEFT JOIN
                        OrderItems oi ON (oi.OrderId = o.Id)
                    WHERE
                        o.Id = @Id
                        AND o.DateTimeDeleted IS NULL
                    ";

            OrderDTO order = null;
            var ordersDictionary = new Dictionary<int, OrderDTO>();
            using (var connection = GetConnection())
            {
                order = connection.Query<OrderDTO, OrderItem, OrderDTO>(
                    query,
                    (order, orderItem) =>
                    {
                        OrderDTO orderDTO;
                        if (!ordersDictionary.TryGetValue(order.Id, out orderDTO))
                        {
                            orderDTO = order;
                            orderDTO.OrderItems = new List<OrderItem>();
                            ordersDictionary.Add(orderDTO.Id, orderDTO);
                        }

                        if (orderItem != null)
                        {
                            orderDTO.OrderItems.Add(orderItem);
                        }
                        return orderDTO;
                    },
                    new { Id = id })
                    .FirstOrDefault();
            }

            return order;
        }

        public IEnumerable<OrderDTO> List(int page, int size)
        {
            var offset = (page - 1) * size;
            string query = $@"
                    SELECT * FROM
                        Orders o
                    WHERE
                        o.DateTimeDeleted IS NULL
                    ORDER BY
                        Id
                    OFFSET {offset} ROWS
                    FETCH NEXT {size} ROWS ONLY
                    ";

            List<OrderDTO> orders = null;
            using (var connection = GetConnection())
            {
                orders = connection.Query<OrderDTO>(query).ToList();
            }

            return orders;
        }

        public int UpdateDeliveryAddress(Order order)
        {
            string query = @"
                    UPDATE
                        Orders
                    SET
                        AddressLine1 = @AddressLine1,
                        AddressLine2 = @AddressLine2,
                        AddressLine3 = @AddressLine3,
                        AddressLine4 = @AddressLine4,
                        PostalCode = @PostalCode,
                        CountryCode = @CountryCode,
                        DateTimeUpdated = @DateTimeUpdated
                    WHERE
                        Id = @Id;
                    ";

            var result = 0;
            using (var connection = GetConnection())
            {
                result = connection.Execute(query, order);
            }

            return result;
        }
    }
}
