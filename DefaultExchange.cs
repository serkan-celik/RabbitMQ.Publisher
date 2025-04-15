
using RabbitMQ.Client;
using System.Text;

//RabbitMQ Cloud Bağlantı Adresi Oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqp://guest:guest@localhost:5672");

//Bağlantıyı Aktifleştirme ve Kanal Açma
using IConnection connection = factory.CreateConnection();

//Kanal Açma
using IModel channel = connection.CreateModel();

//Queue(Kuyruk) Oluşturma
//exclusive: parametresi bu kuyruğun özel olup olmadığı yani birden fazla bağlantıyla bu kuyrukta işlem yapıp yapamayacağımızı belirler. Varsayılan true' dur. Yani bu bağlantının dışında başka bir bağlantı bu kuyruğu kullanamayacaktır demektir. Consumerlarla bağlanıp işlem yapacağımız için false diyoruz.
//autoDelete:true tüketicilerin okuma işi bittiğinde kuyruk silinir.
channel.QueueDeclare(queue: "example-queue", exclusive: false);

//Queue(Kuyruk)'ye Mesaj Gönderme
/*onceki
 * 
byte[] message = Encoding.UTF8.GetBytes("Merhaba");

//exchange: "" => Default Direk Exchange dir. O yüzden routingKey queue adıyla aynıdır.
await channel.BasicPublishAsync(exchange: "", routingKey: "example-queue", body: message);
*/

//IBasicProperties properties = channel.CreateBasicProperties();
//properties.Persistent = true;

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba " + i);
    //exchange: "", yani default exchange Direct exchange'e karşılık gelir. Direct exchange'de routing key mesaj kuyruğunun adına karşılık gelir.
    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message);
}

Console.Read();