using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MQTT_Intek
{
    internal class Message
    {
        [JsonPropertyName("topic")]
        public readonly string _topic;
        [JsonPropertyName("sender")]
        public readonly string _sender;
        [JsonPropertyName("content")]
        public readonly string _content;
        [JsonPropertyName("timestamp")]
        public DateTime _timestamp { get; set; }

        public Message(string sender, string content, string topic)
        {
            _topic = topic;
            _sender = sender;
            _content = content;
            _timestamp = DateTime.Now;
        }
    }

    internal class MessageJsonConverter : JsonConverter<Message>
    {
        public override Message Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            string topic = null;
            string sender = null;
            string content = null;
            DateTime timestamp = DateTime.MinValue;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return new Message(sender, content, topic) { _timestamp = timestamp };
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();

                    switch (propertyName)
                    {
                        case "topic":
                            topic = reader.GetString();
                            break;
                        case "sender":
                            sender = reader.GetString();
                            break;
                        case "content":
                            content = reader.GetString();
                            break;
                        case "timestamp":
                            timestamp = reader.GetDateTime();
                            break;
                    }
                }
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Message value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("topic", value._topic);
            writer.WriteString("sender", value._sender);
            writer.WriteString("content", value._content);
            writer.WriteString("timestamp", value._timestamp);
            writer.WriteEndObject();
        }
    }
}