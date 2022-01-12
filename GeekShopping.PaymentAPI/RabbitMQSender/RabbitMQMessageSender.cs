using GeekShopping.MessageBus;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System;
using GeekShopping.PaymentAPI.Messages;

namespace GeekShopping.PaymentAPI.RabbitMQSender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private const string EXCHANGE_NAME = "FanoutPaymentUpdateExchange";

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

        public void SendMessage(BaseMessage message)
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();
                channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Fanout, durable: false);

                byte[] body = GetMessageAsByArray(message);
                channel.BasicPublish(exchange: EXCHANGE_NAME, routingKey: "", basicProperties: null, body: body);
            }
        }

        private byte[] GetMessageAsByArray(BaseMessage message)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize<UpdatePaymentResultMessage>((UpdatePaymentResultMessage)message, options);
            var body = Encoding.UTF8.GetBytes(json);

            return body;
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password,
                    Port = _port
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null) return true;

            CreateConnection();

            return _connection != null;
        }

    }
}
