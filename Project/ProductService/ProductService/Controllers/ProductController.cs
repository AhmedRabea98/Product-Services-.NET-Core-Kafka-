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
        private readonly string bootstrapServers = "localhost:9092";
        private readonly string topic = "simpletalk_topic";

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            string messsage = JsonSerializer.Serialize(product);
            return Ok(await SendOrderRequest(topic, messsage));
        }

        public async Task<bool> SendOrderRequest(string topic, string message)
        {
            ProducerConfig config = new ProducerConfig
            {

                BootstrapServers = bootstrapServers,
                ClientId = Dns.GetHostName(),

            };
            try
            {
                using (var producer = new ProducerBuilder<Null, string>(config).Build())
                {

                    var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
                    Console.WriteLine($"Delivery time {result.Timestamp.UtcDateTime}");
                    return await Task.FromResult(true);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error {ex.Message}");
            }
            return await Task.FromResult(false);
        }

    }
}
