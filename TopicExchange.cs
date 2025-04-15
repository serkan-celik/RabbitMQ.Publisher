
using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqp://guest:guest@localhost:5672");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "topic-exchange", 
    type: ExchangeType.Topic);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");

    Console.Write("Mesajın belirtileceği topic formatını belirtiniz : ");
    string topic = Console.ReadLine();

    channel.BasicPublish(
        exchange: "topic-exchange", 
        routingKey: topic, 
        body: message);
}

Console.Read();