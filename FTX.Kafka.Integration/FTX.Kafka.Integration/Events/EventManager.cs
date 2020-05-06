using FTX.Kafka.Integration.Configuration;
using FTX.Kafka.Integration.Kafka;
using FTX.Kafka.Integration.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FTX.Kafka.Integration.Events
{
    public class EventManager : BackgroundService
    {
        private readonly ILogger<EventManager> _logger;
        private readonly KafkaConsumer _consumer;
        private readonly ConsumerConfiguration _configuration;
        //private readonly TestEntityFContext _dbcontext;

        public EventManager(ILoggerFactory loggerFactory, ConsumerConfiguration consumerConfiguration
            //,TestEntityFContext dbContext
            )
        {
            _logger = loggerFactory.CreateLogger<EventManager>();
            _configuration = consumerConfiguration;
            _consumer = new KafkaConsumer(loggerFactory.CreateLogger<KafkaConsumer>(), _configuration);
            //_dbcontext = dbContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting EventManager service. Consuming to the following topics [{string.Join(", ", _configuration.Topics)}]");

            await Task.Run(() =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    //_consumer.Consume(stoppingToken);
                    string contactString = _consumer.ConsumeReadMessage(stoppingToken);
                    Contact contactTostore = JsonConvert.DeserializeObject<Contact>(contactString);

                    try
                    {
                        using (TestEntityFContext context = new TestEntityFContext())
                        {
                            context.Contact.Add(contactTostore);
                            context.SaveChanges();
                        }
                    }
                    catch (Exception)
                    {
                    }
                   
                   
                }
            }, stoppingToken);
        }
    }
}
