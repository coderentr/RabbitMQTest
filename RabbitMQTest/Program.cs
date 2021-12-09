using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Person person = new Person() { Name = "Mustafa", SurName = "EREN", ID = 1, BirthDate = new DateTime(1994, 05, 24),
                Message = "İlgili aday yakınımdır :)" };
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Coderen",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = JsonConvert.SerializeObject(person);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "",
                                     routingKey: "Coderen",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine($"Gönderilen kişi: {person.Name}-{person.SurName}");
            }

            Console.WriteLine(" İlgili kişi gönderildi...");
            Console.ReadLine();
        }
        public class Person
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string SurName { get; set; }
            public DateTime BirthDate { get; set; }
            public string Message { get; set; }
        }   
    }
}
