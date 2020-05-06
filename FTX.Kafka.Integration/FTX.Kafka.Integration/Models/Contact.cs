using System;
using System.Collections.Generic;

namespace FTX.Kafka.Integration.Models
{
    public partial class Contact
    {
        public int Id { get; set; }
        public int? CustomeId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}
