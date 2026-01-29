using System;
using System.Collections.Generic;

namespace RabbitMQExplorer.Models
{
    public class MessageInfo
    {
        public string MessageId { get; set; } = Guid.NewGuid().ToString();
        public string Body { get; set; } = string.Empty;
        public DateTime ReceivedAt { get; set; } = DateTime.Now;
        public string RoutingKey { get; set; } = string.Empty;
        public string Exchange { get; set; } = string.Empty;
        public string Queue { get; set; } = string.Empty;
        public bool Redelivered { get; set; }
        public Dictionary<string, object> Headers { get; set; } = new();
        public string ContentType { get; set; } = "text/plain";
        public string ContentEncoding { get; set; } = "UTF-8";
        public byte DeliveryMode { get; set; } = 1; // 1=non-persistent, 2=persistent
        public byte Priority { get; set; } = 0;
        public string? CorrelationId { get; set; }
        public string? ReplyTo { get; set; }
        public string? Expiration { get; set; }
        public string? Type { get; set; }
        public string? UserId { get; set; }
        public string? AppId { get; set; }
        public ulong DeliveryTag { get; set; }
    }
}
