using Confluent.Kafka;
using PaymentService.Context;
using System.Text.Json;

namespace PaymentService.Models
{
    public class ProductConsumer : BackgroundService
    {
        private readonly string Topic = "simpletalk_topic";
        private readonly string GroupID = "product_group";
        private readonly string bootstrapservers = "localhost:9092";
        private readonly IServiceScopeFactory scopeFactory;
        public ProductConsumer(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapservers,
                GroupId = GroupID,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            using (var scope = scopeFactory.CreateScope())
            {
                var dbcontext = scope.ServiceProvider.GetRequiredService<DBCONTEXT>();
                using (var consumerBuilder = new ConsumerBuilder<Ignore,string>(config).Build())
                {
                    consumerBuilder.Subscribe(Topic);
                    try 
                    {
                        await Task.Run(async () => 
                        {
                            while (!stoppingToken.IsCancellationRequested)
                            {

                                var consumer = consumerBuilder.Consume(stoppingToken);
                                OrderRequest orderRequest = JsonSerializer.Deserialize<OrderRequest>(consumer.Message.Value);
                                Console.WriteLine($"Processing Order, OrderId : {orderRequest.OrderId}" +
                                    $" ProductId :{ orderRequest.ProductId} CustomerId : {orderRequest.CustomerId}" +
                                    $"Quantity : {orderRequest.Quantity} Satus : {orderRequest.Status = "Ordered"}");

                                try {
                                    orderRequest.Status = "ordered";
                                    orderRequest.Payment = orderRequest.Quantity * 50;
                                    dbcontext.Orders.Add(orderRequest);
                                    dbcontext.SaveChanges();
                                    Console.WriteLine("Added Successfully");

                                }
                                catch (Exception ex)
                                {

                                    Console.WriteLine(ex.ToString());
                                }

                            }
                        
                        },stoppingToken);
                    }
                    catch (OperationCanceledException)
                    {
                        consumerBuilder.Close();
                    }

                }


            }
           
        }
    }
}
