using System;
using System.Threading;
using Confluent.Kafka;

namespace KafkaConsumer
{
    class Program
    {
        static void Main()
        {
           // var conf = new ConsumerConfig
           // { 
           //     GroupId = "test-consumer-group",
           //     BootstrapServers = "localhost:9092",
           //     AutoOffsetReset = AutoOffsetReset.Earliest
           //};

            var conf = new ConsumerConfig
            {
                BootstrapServers = "omnibus-01.srvs.cloudkafka.com:9094,omnibus-02.srvs.cloudkafka.com:9094,omnibus-03.srvs.cloudkafka.com:9094",
                SaslMechanism = SaslMechanism.ScramSha256,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = "3nxao1b5",
                SaslPassword = "VDxpjkxLUalRRAty__w8PzTIdFajRwIH",
                GroupId = "test-consumer-group",//Guid.NewGuid().ToString(),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var c = new ConsumerBuilder<Ignore, string>(conf).Build();
            c.Subscribe("3nxao1b5-default");//test

            // Because Consume is a blocking call, we want to capture Ctrl+C and use a cancellation token to get out of our while loop and close the consumer gracefully.
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => {
                e.Cancel = true;
                cts.Cancel();
            };

            try
            {
                while (true)
                {
                    // Consume a message from the test topic.  Pass in a cancellation token so we can break out of our loop when Ctrl+C is pressed
                    var cr = c.Consume(cts.Token);
                    Console.WriteLine($"Consumed message '{cr.Value}' from topic {cr.Topic}, partition {cr.Partition}, offset {cr.Offset}");

                    // Do something interesting with the message you consumed
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                c.Close();
            }
        }
    }
}
