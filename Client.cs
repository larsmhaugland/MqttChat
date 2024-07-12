using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using static MQTT_Intek.Message;

namespace MQTT_Intek
{
    internal class Client
    {
        /// <summary>
        /// The hostname of the broker to connect to
        /// </summary>
        private string _brokerHostname;
        /// <summary>
        /// The client ID for the connection
        /// </summary>
        private string _clientId;
        /// <summary>
        /// The port to connect to the broker on
        /// </summary>
        private int _port;
        /// <summary>
        /// Whether or not to reconnect after a disconnect
        /// </summary>
        private bool _reconnect;
        /// <summary>
        /// The MQTT client object
        /// </summary>
        private IMqttClient _mqttClient;
        /// <summary>
        /// A list of topics that the client is subscribed to
        /// </summary>
        private List<string> _subscriptions;
        /// <summary>
        /// A dictionary of topics and their stored messages
        /// </summary>
        private Dictionary<string,List<Message>> _messages;
        /// <summary>
        /// An event handler for when a message is received
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        /// <summary>
        /// Constructor for creating a new MQTT client with credentials
        /// </summary>
        /// <param name="brokerHostname">Hostname of broker</param>
        /// <param name="clientId">Client ID for connections</param>
        /// <param name="port">Port to connect to broker</param>
        public Client(string brokerHostname, string clientId, int port = 1883)
        {
            // Set the fields to the provided values
            _brokerHostname = brokerHostname;
            _clientId = clientId;
            _port = port;
            _messages = new Dictionary<string, List<Message>>();
            _mqttClient = new MqttFactory().CreateMqttClient();
            _subscriptions = new List<string>();

            // Connect to the broker
            Connect().Wait();

            // Set up event handlers
            _mqttClient.DisconnectedAsync += Client_MqttDisconnected;
            _mqttClient.ApplicationMessageReceivedAsync += Client_MqttMsgPublishReceived;
        }

        /// <summary>
        /// Subscribes to a topic with a given QoS level
        /// </summary>
        /// <param name="topic">Topic to subscribe to</param>
        /// <param name="qosLevel">QoS level for subscribtion, default is ExactlyOnce (2)</param>
        public void Subscribe(string topic, MqttQualityOfServiceLevel qosLevel = MqttQualityOfServiceLevel.ExactlyOnce)
        {
            // Set up the subscription options
            MqttClientSubscribeOptions subscribeOptions = new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter(topic, qosLevel)
                .Build();

            // Subscribe to the topic
            _mqttClient.SubscribeAsync(subscribeOptions).Wait();
            _subscriptions.Add(topic);
        }

        /// <summary>
        /// Unsubscribes from a given topic
        /// </summary>
        /// <param name="topic">Topic to unsubscribe from</param>
        public void Unsubscribe(string topic)
        {
            // Set up the unsubscribe options
            MqttClientUnsubscribeOptions unsubscribeOptions = new MqttClientUnsubscribeOptionsBuilder()
                .WithTopicFilter(topic)
                .Build();
            if(_subscriptions.Contains(topic))
            {
                _subscriptions.Remove(topic);
            }
            // Unsubscribe from the topic
            _mqttClient.UnsubscribeAsync(unsubscribeOptions);
        }

        /// <summary>
        /// Checks if the client is connected to the broker
        /// </summary>
        /// <returns>True if client is connected, otherwise False</returns>
        public bool IsConnected()
        {
            return _mqttClient.IsConnected;
        }

        /// <summary>
        /// Publishes a message to a given topic
        /// </summary>
        /// <param name="content">Content of the message</param>
        /// <param name="topic">Topic the message should be sent to, default is MqttChat/{Client ID}</param>
#nullable enable
        public void SendMessage(string content, string? topic)
        {
            // Set the topic to the default if not provided
            if (topic == null)
            {
                topic = $"MqttChat/{_clientId}";
            }
            // Replace wildcards with client ID
            topic.Replace("#", _clientId);
            topic.Replace("+", _clientId);

            // Create a new message object
            Message message = new Message(_clientId, content, topic, DateTime.Now);

            // Setup the custom serializer
            var options = new JsonSerializerOptions();
            options.Converters.Add(new MessageJsonConverter());
            
            // Serialize the message to JSON
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(message, options);
            // Create a new ArraySegment for the payload
            ArraySegment<byte> payloadSegment = new ArraySegment<byte>(json);

            // Create a new message builder
            MqttApplicationMessageBuilder messageBuilder = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayloadSegment(payloadSegment);

            // Publish the message
            _mqttClient.PublishAsync(messageBuilder.Build());
        }
#nullable disable

        /// <summary>
        /// Gets a list of all stored messages for a given topic
        /// </summary>
        /// <param name="topic">Topic to get messages from</param>
        /// <returns>A list of Message stored in the specified topic, an empty list if none were found</returns>
        public List<Message> GetMessages(string topic)
        {
            // Check if the topic is in the dictionary
            if (_messages.ContainsKey(topic))
            {
                return _messages[topic];
            }
            // If the topic is not in the dictionary, check for wildcards
            // Split the topic into parts
            string[] splits = topic.Split('/');
            if (splits.Length < 1)
            {
                return new List<Message>();
            }
            // Check for wildcards
            if (splits.Contains("#") || splits.Contains("*"))
            {
                // Join the parts of the topic until a multi-level wildcard
                string searchString = string.Join("/", splits.TakeWhile((c) => c != "#"));
                // Search for messages that match the wildcard
                return GetMessageWildcard(searchString);
            }

            return new List<Message>();
        }

        /// <summary>
        /// Gets a list of all stored messages for a topic with wildcards
        /// </summary>
        /// <param name="topic">Topic with wildcards to collect messages from</param>
        /// <returns>A list of Message stored in the specified topic, an empty list if none were found</returns>
        private List<Message> GetMessageWildcard(string topic)
        {
            List<Message> messages = new List<Message>();
            
            foreach (var key in _messages.Keys)
            {
                // Use the MqttTopicMatcher to check if the key matches the topic
                if (MqttTopicMatcher.IsMatch(topic,key))
                {
                    messages.AddRange(_messages[key]);
                }
            }
            // Return the list of messages
            return messages;
        }

        /// <summary>
        /// Connects to a broker with the stored options
        /// </summary>
        /// <returns>A Task representing the asynchronous connect operation</returns>
        private Task Connect()
        {
            // Set reconnect to true to allow for automatic reconnection
            _reconnect = true;

            // Create client options builder
            MqttClientOptionsBuilder clientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(_brokerHostname, _port)
                .WithClientId(_clientId)
                .WithCleanSession();

            // Connect to the broker
            return _mqttClient.ConnectAsync(clientOptions.Build());
        }

        /// <summary>
        /// Disconnects from the broker
        /// </summary>
        public void Disconnect()
        {
            // Set reconnect flag to false to prevent automatic reconnection
            _reconnect = false;
            _mqttClient.DisconnectAsync().Wait();
        }

        /// <summary>
        /// Attempts to reconnect to the broker after a disconnect as long as the reconnect flag is set
        /// </summary>
        /// <param name="e">The EventArgs for the disconnection</param>
        /// <returns>A Task representing the asynchronous connect operation or CompletedTask if the _reconnect flag is not set</returns>
        private Task Client_MqttDisconnected(MqttClientDisconnectedEventArgs e)
        {
            // Only reconnect if the reconnect flag is set
            if (_reconnect)
            {
                return Connect();
            }
            // If the reconnect flag is not set, return a CompletedTask
            return Task.CompletedTask;
        }

        /// <summary>
        /// Handles incoming messages from the broker
        /// </summary>
        /// <param name="e">The EventArgs for the received message</param>
        /// <returns>A CompletedTask</returns>
        private Task Client_MqttMsgPublishReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            try { 
                // Setup custom serializer
                var options = new JsonSerializerOptions();
                options.Converters.Add(new MessageJsonConverter());

                // Deserialize the JSON payload into a Message object
                Message message = JsonSerializer.Deserialize<Message>(e.ApplicationMessage.PayloadSegment, options);

                if (message != null)
                {
                    // Add message to dictionary
                    if (!_messages.ContainsKey(message.Topic))
                    {
                        _messages.Add(message.Topic, new List<Message>());
                    }
                    _messages[message.Topic].Add(message);

                    // Invoke the MessageReceived event
                    MessageReceived?.Invoke(this, new MessageReceivedEventArgs(message));
                }
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"Failed to deserialize message: {ex.Message}");
            }
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// A custom EventArgs class for passing a received message to the event handler
    /// </summary>
    internal class MessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// The Message object that was received
        /// </summary>
        public Message Message_ { get; }

        /// <summary>
        /// Constructor for creating a new MessageReceivedEventArgs object
        /// </summary>
        /// <param name="message">The Message object to create an EventArgs for</param>
        public MessageReceivedEventArgs(Message message) => Message_ = message;
    }
}
