using System.Collections.Generic;

namespace RabbitMQExplorer.Models
{
    public class ExchangeInfo
    {
        public string Name { get; set; } = string.Empty;
        public string VHost { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // direct, fanout, topic, headers
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
        public bool Internal { get; set; }
        public Dictionary<string, object> Arguments { get; set; } = new();
        public int MessagePublishIn { get; set; }
        public int MessagePublishOut { get; set; }
    }
}
