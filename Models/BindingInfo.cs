using System.Collections.Generic;

namespace RabbitMQExplorer.Models
{
    public class BindingInfo
    {
        public string Source { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string DestinationType { get; set; } = string.Empty; // queue or exchange
        public string RoutingKey { get; set; } = string.Empty;
        public Dictionary<string, object> Arguments { get; set; } = new();
        public string VHost { get; set; } = string.Empty;
    }
}
