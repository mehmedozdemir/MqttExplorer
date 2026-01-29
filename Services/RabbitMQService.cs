using RabbitMQ.Client;
using RabbitMQExplorer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RabbitMQExplorer.Services
{
    public class RabbitMQService : IDisposable
    {
        private IConnection? _connection;
        private IModel? _channel;
        private readonly HttpClient _httpClient;
        private ConnectionProfile? _currentProfile;

        public bool IsConnected => _connection?.IsOpen ?? false;
        public ConnectionProfile? CurrentProfile => _currentProfile;

        public RabbitMQService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> ConnectAsync(ConnectionProfile profile)
        {
            try
            {
                await Task.Run(() =>
                {
                    var factory = new ConnectionFactory()
                    {
                        HostName = profile.HostName,
                        Port = profile.Port,
                        VirtualHost = profile.VirtualHost,
                        UserName = profile.UserName,
                        Password = profile.Password,
                        RequestedHeartbeat = TimeSpan.FromSeconds(60),
                        AutomaticRecoveryEnabled = true,
                        NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
                    };

                    if (profile.UseSsl)
                    {
                        factory.Ssl.Enabled = true;
                        factory.Ssl.ServerName = profile.HostName;
                    }

                    _connection = factory.CreateConnection($"RabbitMQ Explorer - {profile.Name}");
                    _channel = _connection.CreateModel();
                });

                _currentProfile = profile;
                profile.LastConnected = DateTime.Now;

                // Setup HTTP client for Management API
                var credentials = Convert.ToBase64String(
                    Encoding.ASCII.GetBytes($"{profile.UserName}:{profile.Password}"));
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Bağlantı hatası: {ex.Message}", ex);
            }
        }

        public void Disconnect()
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
            _channel = null;
            _connection = null;
            _currentProfile = null;
        }

        public async Task<List<QueueInfo>> GetQueuesAsync()
        {
            if (_currentProfile == null) throw new InvalidOperationException("Bağlantı yok!");

            try
            {
                var url = $"{_currentProfile.GetManagementUrl()}/queues/{Uri.EscapeDataString(_currentProfile.VirtualHost)}";
                var response = await _httpClient.GetStringAsync(url);
                var queues = JsonSerializer.Deserialize<List<JsonElement>>(response);

                var result = new List<QueueInfo>();
                if (queues != null)
                {
                    foreach (var q in queues)
                    {
                        var queueInfo = new QueueInfo
                        {
                            Name = q.GetProperty("name").GetString() ?? "",
                            VHost = q.GetProperty("vhost").GetString() ?? "",
                            Durable = q.GetProperty("durable").GetBoolean(),
                            AutoDelete = q.GetProperty("auto_delete").GetBoolean(),
                            Messages = q.TryGetProperty("messages", out var msgs) ? msgs.GetInt32() : 0,
                            MessagesReady = q.TryGetProperty("messages_ready", out var ready) ? ready.GetInt32() : 0,
                            MessagesUnacknowledged = q.TryGetProperty("messages_unacknowledged", out var unack) ? unack.GetInt32() : 0,
                            Consumers = q.TryGetProperty("consumers", out var cons) ? cons.GetInt32() : 0,
                            State = q.TryGetProperty("state", out var state) ? state.GetString() ?? "" : ""
                        };

                        if (q.TryGetProperty("arguments", out var args))
                        {
                            queueInfo.Arguments = JsonSerializer.Deserialize<Dictionary<string, object>>(args.GetRawText()) ?? new();
                            
                            // Parse Dead Letter Queue info
                            if (queueInfo.Arguments.TryGetValue("x-dead-letter-exchange", out var dlx))
                                queueInfo.DeadLetterExchange = dlx?.ToString();
                            
                            if (queueInfo.Arguments.TryGetValue("x-dead-letter-routing-key", out var dlrk))
                                queueInfo.DeadLetterRoutingKey = dlrk?.ToString();
                            
                            if (queueInfo.Arguments.TryGetValue("x-message-ttl", out var ttl))
                                queueInfo.MessageTtl = Convert.ToInt32(ttl);
                            
                            if (queueInfo.Arguments.TryGetValue("x-max-length", out var maxLen))
                                queueInfo.MaxLength = Convert.ToInt32(maxLen);
                        }

                        result.Add(queueInfo);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Kuyruklar alınamadı: {ex.Message}", ex);
            }
        }

        public async Task<List<ExchangeInfo>> GetExchangesAsync()
        {
            if (_currentProfile == null) throw new InvalidOperationException("Bağlantı yok!");

            try
            {
                var url = $"{_currentProfile.GetManagementUrl()}/exchanges/{Uri.EscapeDataString(_currentProfile.VirtualHost)}";
                var response = await _httpClient.GetStringAsync(url);
                var exchanges = JsonSerializer.Deserialize<List<JsonElement>>(response);

                var result = new List<ExchangeInfo>();
                if (exchanges != null)
                {
                    foreach (var e in exchanges)
                    {
                        var exchangeInfo = new ExchangeInfo
                        {
                            Name = e.GetProperty("name").GetString() ?? "",
                            VHost = e.GetProperty("vhost").GetString() ?? "",
                            Type = e.GetProperty("type").GetString() ?? "",
                            Durable = e.GetProperty("durable").GetBoolean(),
                            AutoDelete = e.GetProperty("auto_delete").GetBoolean(),
                            Internal = e.GetProperty("internal").GetBoolean(),
                            MessagePublishIn = e.TryGetProperty("message_stats", out var stats) &&
                                             stats.TryGetProperty("publish_in", out var pubIn) ? pubIn.GetInt32() : 0,
                            MessagePublishOut = e.TryGetProperty("message_stats", out var stats2) &&
                                              stats2.TryGetProperty("publish_out", out var pubOut) ? pubOut.GetInt32() : 0
                        };

                        if (e.TryGetProperty("arguments", out var args))
                        {
                            exchangeInfo.Arguments = JsonSerializer.Deserialize<Dictionary<string, object>>(args.GetRawText()) ?? new();
                        }

                        result.Add(exchangeInfo);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Exchange'ler alınamadı: {ex.Message}", ex);
            }
        }

        public async Task<List<BindingInfo>> GetBindingsAsync()
        {
            if (_currentProfile == null) throw new InvalidOperationException("Bağlantı yok!");

            try
            {
                var url = $"{_currentProfile.GetManagementUrl()}/bindings/{Uri.EscapeDataString(_currentProfile.VirtualHost)}";
                var response = await _httpClient.GetStringAsync(url);
                var bindings = JsonSerializer.Deserialize<List<JsonElement>>(response);

                var result = new List<BindingInfo>();
                if (bindings != null)
                {
                    foreach (var b in bindings)
                    {
                        var bindingInfo = new BindingInfo
                        {
                            Source = b.GetProperty("source").GetString() ?? "",
                            Destination = b.GetProperty("destination").GetString() ?? "",
                            DestinationType = b.GetProperty("destination_type").GetString() ?? "",
                            RoutingKey = b.GetProperty("routing_key").GetString() ?? "",
                            VHost = b.GetProperty("vhost").GetString() ?? ""
                        };

                        if (b.TryGetProperty("arguments", out var args))
                        {
                            bindingInfo.Arguments = JsonSerializer.Deserialize<Dictionary<string, object>>(args.GetRawText()) ?? new();
                        }

                        result.Add(bindingInfo);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Binding'ler alınamadı: {ex.Message}", ex);
            }
        }

        public async Task PublishMessageAsync(string exchange, string routingKey, string message, 
            Dictionary<string, object>? headers = null, byte deliveryMode = 1, byte priority = 0,
            string contentType = "text/plain")
        {
            if (_channel == null) throw new InvalidOperationException("Kanal yok!");

            await Task.Run(() =>
            {
                var properties = _channel.CreateBasicProperties();
                properties.ContentType = contentType;
                properties.DeliveryMode = deliveryMode;
                properties.Priority = priority;
                properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                properties.MessageId = Guid.NewGuid().ToString();

                if (headers != null && headers.Count > 0)
                {
                    properties.Headers = new Dictionary<string, object>(headers);
                }

                var body = Encoding.UTF8.GetBytes(message);
                _channel.BasicPublish(exchange, routingKey, properties, body);
            });
        }

        public IModel? GetChannel() => _channel;

        public void Dispose()
        {
            Disconnect();
            _httpClient?.Dispose();
        }
    }
}
