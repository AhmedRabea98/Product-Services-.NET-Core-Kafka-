using Confluent.Kafka;
using System.Net;

namespace ProductService2
{
    public class ProductProducerConfig
    {
        private readonly string bootstrapServers = "localhost:9092"; 
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
