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

namespace MQTT_Intek
{
    internal class Client
    {
        private string _Server;
        private string _Username;
        private IMqttClient _MqttClient;
        private Dictionary<string,List<Message>> _Messages;

        public Client(string server, string username, int port = 1883)
        {
            _Server = server;
            _Username = username;
            _Messages = new Dictionary<string, List<Message>>();
            _MqttClient = new MqttFactory().CreateMqttClient();

            MqttClientOptions clientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(_Server, port)
                //.WithCredentials("test1", "Password123")
                .WithClientId(_Username)
                .WithCleanSession()
                .Build();

            _MqttClient.ConnectAsync(clientOptions).Wait();
            _MqttClient.ApplicationMessageReceivedAsync += Client_MqttMsgPublishReceived;
        }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public void Subscribe(string topic, MqttQualityOfServiceLevel qosLevel = MqttQualityOfServiceLevel.ExactlyOnce)
        {
            MqttClientSubscribeOptions subscribeOptions= new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter(topic, qosLevel).Build();

            _MqttClient.SubscribeAsync(subscribeOptions).Wait();
            Debug.WriteLine($"Subscribed to topic {topic}");
        }

        public void Unsubscribe(string topic)
        {
            MqttClientUnsubscribeOptions unsubscribeOptions = new MqttClientUnsubscribeOptionsBuilder().WithTopicFilter(topic).Build();
            _MqttClient.UnsubscribeAsync(unsubscribeOptions);
        }

        public bool IsConnected()
        {
            return _MqttClient.IsConnected;
        }

        public void SendMessage(string content, string topic = "#")
        {
            if (topic.Contains('#'))
            {
                topic = $"MqttChat/{_Username}";
            }

            Message message = new Message(_Username, content, topic);
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(message);
            MqttApplicationMessageBuilder messageBuilder = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(json);

            _MqttClient.PublishAsync(messageBuilder.Build());
        }

        public List<Message> GetMessages(string topic)
        {
            if (_Messages.ContainsKey(topic))
            {
                return _Messages[topic];
            }
            string[] splits = topic.Split('/');
            if (splits.Length < 1)
            {
                return new List<Message>();
            }
            if (splits.Contains("#"))
            {
                return GetMessageWildcard(topic);
            }

            return new List<Message>();
        }

        public List<Message> GetMessageWildcard(string topic)
        {
            List<Message> messages = new List<Message>();
            foreach (var key in _Messages.Keys)
            {
                if (key.StartsWith(topic))
                {
                    messages.AddRange(_Messages[key]);
                }
            }
            return messages;
        }

        private Task Client_MqttMsgPublishReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            
            string text = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment.Array);
            var options = new JsonSerializerOptions();
            options.Converters.Add(new MessageJsonConverter());

            Message message = JsonSerializer.Deserialize<Message>(text, options);
            string topic = message._topic;

            if (topic == null)
            {
                topic = e.ApplicationMessage.Topic;
            }

            if (!_Messages.ContainsKey(topic))
            {
                _Messages.Add(topic, new List<Message>());               
            }
            _Messages[topic].Add(message);
            
            Debug.WriteLine($"Message received from {e.ClientId} on topic {e.ApplicationMessage.Topic}");
            MessageReceived?.Invoke(this, new MessageReceivedEventArgs(e.ApplicationMessage.Topic, message));
            return Task.CompletedTask;
        }
    }

    // Custom event arguments to pass the message and topic
    internal class MessageReceivedEventArgs : EventArgs
    {
        public string Topic { get; }
        public Message Message_ { get; }

        public MessageReceivedEventArgs(string topic, Message message)
        {
            Topic = topic;
            Message_ = message;
        }
    }

}
