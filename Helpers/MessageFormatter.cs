using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace RabbitMQExplorer.Helpers
{
    public static class MessageFormatter
    {
        public static string FormatMessage(string message, string contentType)
        {
            try
            {
                if (contentType.Contains("json", StringComparison.OrdinalIgnoreCase))
                {
                    return FormatJson(message);
                }
                else if (contentType.Contains("xml", StringComparison.OrdinalIgnoreCase))
                {
                    return FormatXml(message);
                }
            }
            catch
            {
                // If formatting fails, return original
            }

            return message;
        }

        public static string FormatJson(string json)
        {
            try
            {
                var parsedJson = JToken.Parse(json);
                return parsedJson.ToString(Newtonsoft.Json.Formatting.Indented);
            }
            catch
            {
                return json;
            }
        }

        public static string FormatXml(string xml)
        {
            try
            {
                var doc = XDocument.Parse(xml);
                return doc.ToString();
            }
            catch
            {
                return xml;
            }
        }

        public static bool IsJson(string text)
        {
            try
            {
                JToken.Parse(text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsXml(string text)
        {
            try
            {
                XDocument.Parse(text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string DetectContentType(string message)
        {
            if (IsJson(message)) return "application/json";
            if (IsXml(message)) return "application/xml";
            return "text/plain";
        }
    }
}
