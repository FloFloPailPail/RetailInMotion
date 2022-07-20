using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RetailInMotionContracts;
using RetailInMotionContracts.BLL;
using RetailInMotionObjects;
using RetailInMotionObjects.ApiModels;

namespace RetailInMotion.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        //private readonly ILogger<OrderController> _logger;
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService/*, ILogger<OrderController> logger*/)
        {
            _ordersService = ordersService;
            //_logger = logger;
        }

        [HttpPost("add-new")]
        public IActionResult Create()
        {
            var orderId = _ordersService.Create();
            return Ok(orderId);
        }

        [HttpPost("add")]
        public IActionResult Create(OrderApiModel order)
        {
            var createdOrder = _ordersService.Create(order);
            return Ok(createdOrder);
        }

        [HttpPost("{id}/edit-delivery-address")]
        public IActionResult UpdateDeliveryAddress(int id, OrderApiModel order)
        {
            var orderResult = _ordersService.UpdateDeliveryAddress(id, order);
            return Ok(orderResult);
        }

        [HttpPost("{id}/edit-items")]
        public IActionResult UpdateItems(int id, IEnumerable<OrderItemApiModel> orderItems)
        {
            var order = _ordersService.UpdateItems(id, orderItems);
            return Ok(order);
        }

        [HttpDelete("{id}/delete")]
        public IActionResult Cancel(int id)
        {
            var order = _ordersService.Cancel(id);
            return Ok(order);
        }

        [HttpGet("{id}")]
        public ActionResult<OrderApiModel> Get(int id)
        {
            var order = _ordersService.Get(id);
            return Ok(order);
        }

        [HttpGet]
        public ActionResult<IEnumerable<OrderApiModel>> List(int page = 1, int size = 10)
        {
            var orders = _ordersService.List(page, size);
            return Ok(orders);
        }
    }
}
