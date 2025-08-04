using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)  // <-- async ve Task kullanılıyor
    {
        ConnectionFactory factory = new ConnectionFactory()
        {
            Uri = new Uri("amqp://guest:guest@localhost:5672")
        };

        using IConnection connection = await factory.CreateConnectionAsync();
        using var channel = connection.CreateModel();

        string exchangeName = "direct-exchange-example";
        string routingKey = "direct-routing-key";

        channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);

        while (true)
        {
            Console.Write("Mesaj Giriniz: ");
            string message = Console.ReadLine();
            byte[] byteMessage = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: exchangeName,
                routingKey: routingKey,
                basicProperties: null,
                body: byteMessage
            );

            Console.WriteLine($"[x] Gönderildi: {message}");
        }
    }
}
