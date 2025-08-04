using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var factory = new ConnectionFactory()
        {
            Uri = new Uri("amqps://befjdvjy:dBfEg0GaJyIWuF1Yxd8J0z9Cc0sErzk6@moose.rmq.cloudamqp.com/befjdvjy")
        };

        IConnection connection = await factory.CreateConnectionAsync();
        using var channel = connection.CreateModel();

        string exchangeName = "direct-exchange-example";
        string routingKey = "direct-queue-example";
        string queueName = channel.QueueDeclare().QueueName;

        channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);
        channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, e) =>
        {
            var body = e.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"[x] Alındı: {message}");

            // İşlem tamamlandığında Task dön
            await Task.Yield();
        };

        channel.BasicConsume(
            queue: queueName,
            autoAck: true,
            consumer: consumer
        );

        Console.WriteLine("Dinliyor... Çıkmak için Enter'a bas.");
        Console.ReadLine();
    }
}
