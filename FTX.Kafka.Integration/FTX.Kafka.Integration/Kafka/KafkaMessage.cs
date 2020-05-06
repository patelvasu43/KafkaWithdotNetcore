using System;
using System.Collections.Generic;
using System.Text;

namespace FTX.Kafka.Integration.Kafka
{
    public class KafkaMessage
    {
        public string Topic { get; set; }
        public string Message { get; set; }

        //public T Object { get; set; }
    }
}
