using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Text.Json;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
       
        private readonly string topic = "product-topic";
        ProductProducerConfig producerConfig = new ProductProducerConfig();
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            string messsage = JsonSerializer.Serialize(product);
            return Ok(await producerConfig.SendOrderRequest(topic, messsage));
        }

    }
}
