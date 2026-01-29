using System;

namespace RabbitMQExplorer.Models
{
    public class ConnectionProfile
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string VirtualHost { get; set; } = "/";
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public bool UseSsl { get; set; } = false;
        public int ManagementPort { get; set; } = 15672;
        public bool IsDefault { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastConnected { get; set; }

        public string GetConnectionString()
        {
            return $"amqp://{UserName}:{Password}@{HostName}:{Port}{VirtualHost}";
        }

        public string GetManagementUrl()
        {
            var protocol = UseSsl ? "https" : "http";
            return $"{protocol}://{HostName}:{ManagementPort}/api";
        }
    }
}
