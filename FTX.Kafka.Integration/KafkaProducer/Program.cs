using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace KafkaProducer
{
    class Program
    {
        public static async Task Main()
        {
            //var config = new ProducerConfig
            //{
            //    BootstrapServers = "localhost:9092"
            //};

            var config = new ProducerConfig
            {
                BootstrapServers = "omnibus-01.srvs.cloudkafka.com:9094,omnibus-02.srvs.cloudkafka.com:9094,omnibus-03.srvs.cloudkafka.com:9094",
                SaslMechanism = SaslMechanism.ScramSha256,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                // Note: If your root CA certificates are in an unusual location you
                // may need to specify this using the SslCaLocation property.
                SaslUsername = "3nxao1b5",
                SaslPassword = "VDxpjkxLUalRRAty__w8PzTIdFajRwIH",
            };

            

            //new Dictionary<string, object>
            //{
            //    { "group.id", "Vinod_Test_Client.1.Group.1"},
            //    { "client.id", "Vinod_Test_Client.1" },
            //    { "enable.auto.commit", false },
            //    { "auto.commit.interval.ms", 5000 },
            //    { "bootstrap.servers", brokerList },
            //    {"sasl.mechanisms", "SCRAM-SHA-256"},
            //    {"security.protocol", "SASL_PLAINTEXT"},
            //    {"sasl.username", "testuser"},
            //    {"sasl.password", "test1234"},
            //    { "api.version.request", "true" },
            //    { "debug", "cgrp,topic,fetch"},
            //    { "default.topic.config", new Dictionary<string, object>()
            //        {
            //            { "auto.offset.reset", "latest" }
            //        }
            //    }
            //};

            // Create a producer that can be used to send messages to kafka that have no key and a value of type string 
            using var p = new ProducerBuilder<Null, string>(config).Build();

            var i = 0;
            while (true)
            {
                // Construct the message to send (generic type must match what was used above when creating the producer)
                var message = new Message<Null, string>
                {
                    Value = $"Message #{++i}"
                };
                
                // Send the message to our test topic in Kafka
                var dr = await p.ProduceAsync("3nxao1b5-default", message);
                Console.WriteLine($"Delivered '{dr.Value}' to topic {dr.Topic}, partition {dr.Partition}, offset {dr.Offset}");
                
                Thread.Sleep(50);
            }
        }
    }
}
