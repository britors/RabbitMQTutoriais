﻿using RabbitMQ.Client;
using System;
using System.IO;

namespace ProduceFile
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "importacao_produtos",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);



                byte[] body = File.ReadAllBytes(@"..\..\..\File\Example.xlsx");

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                for (var n = 0; n < 1000; n++)
                {
                    channel.BasicPublish(exchange: "",
                                         routingKey: "importacao_produtos",
                                         basicProperties: properties,
                                         body: body);
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
