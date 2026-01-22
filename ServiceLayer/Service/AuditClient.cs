using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Task_Managment_API.ServiceLayer.Dto.AuditDtos;
using Task_Managment_API.ServiceLayer.IService;

namespace Task_Managment_API.ServiceLayer.Service
{
    public class AuditClient : IAuditClient
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IConfiguration _configuration;

        public AuditClient(IConnectionFactory connectionFactory, IConfiguration configuration)
        {
            _connectionFactory = connectionFactory;
            _configuration = configuration;
        }

        public async Task LogAsync(CreateActivityRequest request)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            
            var queueName = _configuration["RabbitMQ:QueueName"];
            
            await channel.QueueDeclareAsync(
                queue: queueName, 
                durable: true, 
                exclusive: false, 
                autoDelete: false, 
                arguments: null);
            
            var message = JsonSerializer.Serialize(request);
            var body = Encoding.UTF8.GetBytes(message);
            
            var properties = new BasicProperties
            {
                Persistent = true
            };
            
            await channel.BasicPublishAsync(
                exchange: "", 
                routingKey: queueName, 
                mandatory: false,
                basicProperties: properties,
                body: body);
        }
    }
}