
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.IO;

namespace RabbitMQ_Receive
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            List<Person> personList = new List<Person>();
            string fileName = @"C:\TestProjeler\Logs.txt";
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Coderen",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Person person = JsonConvert.DeserializeObject<Person>(message);
                    personList.Add(person);
                    File.AppendAllText(fileName, Environment.NewLine + $" Adı: {person.Name} Soyadı:{person.SurName} [{person.Message}]");
                };
                channel.BasicConsume(queue: "Coderen", autoAck: true, consumer: consumer);
                fs.Close();
                Console.ReadLine();
            }
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
