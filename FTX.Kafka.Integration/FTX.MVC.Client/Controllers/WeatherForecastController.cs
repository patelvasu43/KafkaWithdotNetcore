using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FTX.Kafka.Integration.Kafka;
using FTX.Kafka.Integration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FTX.MVC.Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly TestEntityFContext _dbcontext;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly KafkaProducer _producer;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, KafkaProducer producer
            , TestEntityFContext dbContext

            )
        {
            _logger = logger;
            _producer = producer;

        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            var data = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
            return data;
        }

        [HttpPost]
        public async Task Post([FromBody] KafkaMessage kafkaMessage)
        {
            try
            {
                int counterObject = 1;
                while (counterObject <= 5000)
                {
                    Contact contact = new Contact
                    {
                        CustomeId = counterObject,
                        Name = "vasu",
                        Address = "test123",
                        Email = counterObject + "vasumca82@gmail.com",
                    };

                    string serializedContact = JsonConvert.SerializeObject(contact);
                    kafkaMessage.Message = serializedContact;
                    await _producer.ProduceAsync(kafkaMessage.Topic, kafkaMessage.Message);
                    counterObject++;
                }



            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {

            }




        }
    }
}
