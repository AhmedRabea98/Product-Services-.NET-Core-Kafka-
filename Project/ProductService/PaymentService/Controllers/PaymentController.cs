using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Context;
using PaymentService.Models;
using System.Collections.Generic;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly DBCONTEXT DB;
        public PaymentController(DBCONTEXT db)
        {
                this.DB = db;   
        }

        [HttpGet("order")]
        public IActionResult Get()
        {
            List<OrderRequest> orders = DB.Orders.ToList();
            return Ok(orders);
        }

        [HttpGet("GetOrder/{orderid:int}")]
        public IActionResult GetOrder(int orderid)
        {
            OrderRequest order = DB.Orders.Find(orderid);
            return Ok(order);
        }
    }
}
