using System.Collections.Generic;

namespace RabbitMQExplorer.Models
{
    public class QueueInfo
    {
        public string Name { get; set; } = string.Empty;
        public string VHost { get; set; } = string.Empty;
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
        public bool Exclusive { get; set; }
        public int Messages { get; set; }
        public int MessagesReady { get; set; }
        public int MessagesUnacknowledged { get; set; }
        public int Consumers { get; set; }
        public string State { get; set; } = string.Empty;
        public Dictionary<string, object> Arguments { get; set; } = new();
        
        // Dead Letter Queue info
        public string? DeadLetterExchange { get; set; }
        public string? DeadLetterRoutingKey { get; set; }
        public int? MessageTtl { get; set; }
        public int? MaxLength { get; set; }
    }
}
