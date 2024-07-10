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
    /// <summary>
    /// Class to represent a message sent over MQTT.
    /// </summary>
    internal class Message
    {
        /// <summary>
        /// The topic of the message.
        /// </summary>
        public readonly string Topic;
        /// <summary>
        /// The sender of the message.
        /// </summary>
        [JsonPropertyName("sender")]
        public readonly string Sender;
        /// <summary>
        /// The content of the message.
        /// </summary>
        [JsonPropertyName("content")]
        public readonly string Content;
        /// <summary>
        /// The timestamp of when the message was sent.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public readonly DateTime Timestamp;

        /// <summary>
        /// Constructor for a message.
        /// </summary>
        /// <param name="sender">Sender of the message</param>
        /// <param name="content">Content of the message</param>
        /// <param name="topic">Topic the message was sent to</param>
        /// <param name="timestamp">Timestamp of when the message was sent</param>
        public Message(string sender, string content, string topic, DateTime timestamp) =>
        (Sender, Content, Topic, Timestamp) = (sender, content, topic, timestamp);


        /// <summary>
        /// Class to convert a Message object to and from JSON.
        /// </summary>
        internal class MessageJsonConverter : JsonConverter<Message>
        {
            /// <summary>
            /// Reads and converts the JSON to type T.
            /// </summary>
            /// <param name="reader">The reader.</param>
            /// <param name="typeToConvert">The type to convert.</param>
            /// <param name="options">An object that specifies serialization options to use.</param>
            /// <returns>The converted value.</returns>
            /// <exception cref="JsonException"></exception>
            public override Message Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                // If the JSON is not an object, throw an exception
                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException();
                }
                string topic = null;
                string sender = null;
                string content = null;
                DateTime timestamp = DateTime.MinValue;
                // Read the JSON object
                while (reader.Read())
                {
                    // If the JSON object ends, return the Message object
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        return new Message(sender, content, topic, timestamp);
                    }
                    // If the JSON object has a property, read the property name and value
                    if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        string propertyName = reader.GetString();
                        reader.Read();
                        // Set the property value based on the property name
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
                // If the JSON object is not closed, throw an exception
                throw new JsonException();
            }

            /// <summary>
            /// Writes a specified value as JSON.
            /// </summary>
            /// <param name="writer">Writer to write to.</param>
            /// <param name="value">The value to convert to JSON.</param>
            /// <param name="options">An object that specifies serialization options to use.</param>
            public override void Write(Utf8JsonWriter writer, Message value, JsonSerializerOptions options)
            {
                // Write the Message object as a JSON object
                writer.WriteStartObject();
                writer.WriteString("topic", value.Topic);
                writer.WriteString("sender", value.Sender);
                writer.WriteString("content", value.Content);
                writer.WriteString("timestamp", value.Timestamp);
                writer.WriteEndObject();
            }
        }
    }
}