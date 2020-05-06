using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace FTX.Kafka.Integration.Configuration
{
    public class ConsumerConfiguration
    {
        public IEnumerable<string> Topics { get; set; }

        public ConsumerConfig Configuration { get; set; }
    }
}
