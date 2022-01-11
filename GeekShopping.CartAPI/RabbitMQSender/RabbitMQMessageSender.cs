using GeeekShopping.MessageBus;
using GeekShopping.CartAPI.Messages;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace GeekShopping.CartAPI.RabbitMQSender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _username;
        private readonly int _port;
        private IConnection _connection;

        public RabbitMQMessageSender()
        {
            _hostname = "192.168.0.111";
            _password = "guest";
            _username = "guest";
            _port = 5672;
        }

        public void SendMessage(BaseMessage message, string queueName)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password,
                Port = _port
            };
            _connection = factory.CreateConnection();

            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);

            byte[] body = GetMessageAsByArray(message);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }

        private byte[] GetMessageAsByArray(BaseMessage message)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize<CheckoutHeaderVO>((CheckoutHeaderVO)message, options);
            var body = Encoding.UTF8.GetBytes(json);

            return body;
        }
    }
}
