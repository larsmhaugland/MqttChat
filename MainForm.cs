using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MQTT_Intek
{
    public partial class MainForm : Form
    {
        private TextBox txtNewTopic;
        private Button btnSubscribe;
        private ListView listViewTopics;
        private ListView listViewMessages;
        private TextBox txtMessage;
        private Button btnSend;
        private Client _mqttClient;
        private Dictionary<string, List<Message>> topicMessages;
        

        public MainForm()
        {
            InitializeComponent();
            topicMessages = new Dictionary<string, List<Message>>();
        }

        // Event handler for subscribe button click
        private void btnSubscribe_Click(object sender, EventArgs e)
        {
            string topic = txtNewTopic.Text.Trim();
            if (!string.IsNullOrEmpty(topic))
            {
                SubscribeToTopic(topic);
                txtNewTopic.Clear();
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (listViewTopics.SelectedItems.Count > 0)
            {
                string selectedTopic = listViewTopics.SelectedItems[0].Text;
                string messageContent = txtMessage.Text.Trim();
                if (!string.IsNullOrEmpty(messageContent))
                {
                    _mqttClient.SendMessage(messageContent, selectedTopic);
                    txtMessage.Clear();
                }
            }
            else
            {
                MessageBox.Show("Please select a topic to send the message to.", "No Topic Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Event handler for listViewTopics click
        private void listViewTopics_Click(object sender, EventArgs e)
        {
            if (listViewTopics.SelectedItems.Count > 0)
            {
                string selectedTopic = listViewTopics.SelectedItems[0].Text;
                DisplayMessagesForTopic(selectedTopic);
            }
        }

        // Method to subscribe to a topic and add to the list view
        private void SubscribeToTopic(string topic)
        {
            // Add the topic to the ListView
            if (!listViewTopics.Items.ContainsKey(topic))
            {
                listViewTopics.Items.Add(new ListViewItem { Text = topic, Name = topic });
                // Subscribe to the topic (assuming mqttClient is initialized and connected)
                _mqttClient.Subscribe(topic);
            }
        }

        private void DisplayMessagesForTopic(string topic)
        {
            List<Message> messages = _mqttClient.GetMessages(topic);
            listViewMessages.Items.Clear();
            foreach (Message message in messages)
            {
                ListViewItem item = new ListViewItem(new string[] { 
                    message._timestamp.ToString(), 
                    message._sender.ToString(), 
                    message._content.ToString() 
                });

                listViewMessages.Items.Add(item);
            }
        }

        private void SetupMqttClient()
        {
            // Check if the client is already connected
            if (_mqttClient != null && _mqttClient.IsConnected())
            {
                return;
            }
            // Initialize the client with the broker address

            string brokerAddress = "broker.hivemq.com";
            _mqttClient = new Client(brokerAddress, Guid.NewGuid().ToString(), 1883);
            _mqttClient.MessageReceived += MqttClient_MessageReceived;
            SubscribeToTopic("MqttChat/#");
            if (!_mqttClient.IsConnected()) Debug.WriteLine("Client is NOT connected");

        }

        private void MqttClient_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (!topicMessages.ContainsKey(e.Topic))
            {
                topicMessages[e.Topic] = new List<Message>();
            }
            topicMessages[e.Topic].Add(e.Message_);

            // Update the message display if the selected topic is the same as the received topic
            Invoke((MethodInvoker)delegate
            {
                if (listViewTopics.SelectedItems.Count > 0 && listViewTopics.SelectedItems[0].Text == e.Topic)
                {
                    DisplayMessagesForTopic(e.Topic);
                }
            });
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetupMqttClient();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            SetupMqttClient();
        }
    }
}
