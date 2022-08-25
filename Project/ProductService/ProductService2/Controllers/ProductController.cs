using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService2.Models;
using System.Net;
using System.Text.Json;

namespace ProductService2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
   
        private readonly string topic = "product-topic";

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            string messsage = JsonSerializer.Serialize(product);
            ProductProducerConfig producerConfig = new ProductProducerConfig(); 
            return Ok(await producerConfig.SendOrderRequest(topic, messsage));
        }
    }
}
