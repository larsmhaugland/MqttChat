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
    /// <summary>
    /// Class to represent the main form of the application.
    /// </summary>
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
        private int _selectedTopic = -1;

        /// <summary>
        /// Constructor for the main form.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            topicMessages = new Dictionary<string, List<Message>>();
        }

        /// <summary>
        /// Handles the click event for the subscribe button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubscribe_Click(object sender, EventArgs e)
        {
            string topic = txtNewTopic.Text.Trim();
            if (!string.IsNullOrEmpty(topic))
            {
                SubscribeToTopic(topic);
                txtNewTopic.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (_selectedTopic != -1)
            {
                string selectedTopicName = listViewTopics.Items[_selectedTopic].Text;
                string messageContent = txtMessage.Text.Trim();
                if (!string.IsNullOrEmpty(messageContent))
                {
                    _mqttClient.SendMessage(messageContent, selectedTopicName);
                    txtMessage.Clear();
                }
                else
                {
                    MessageBox.Show("Please enter a message to send.", "No Message Entered", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please select a topic to send the message to.", "No Topic Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        /// Handles the click event for the list view of topics.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewTopics_Click(object sender, EventArgs e)
        {
            if (listViewTopics.SelectedItems.Count > 0)
            {
                string selectedTopicName = listViewTopics.SelectedItems[0].Text;
                _selectedTopic = listViewTopics.SelectedItems[0].Index;
                // Highlight selected topic
                for (int i = 0; i < listViewTopics.Items.Count; i++)
                {
                    if (i == _selectedTopic)
                    {
                        listViewTopics.Items[i].BackColor = Color.Cyan;
                        listViewTopics.Items[i].Selected = false;
                    }
                    else
                    {
                        listViewTopics.Items[i].BackColor = Color.White;
                    }
                }
                // Display messages for the selected topic
                DisplayMessagesForTopic(selectedTopicName);
            }
        }

        /// <summary>
        /// Subscribes to a topic and adds it to the list view.
        /// </summary>
        /// <param name="topic">The topic to subscribe to</param>
        private void SubscribeToTopic(string topic)
        {
            // Add the topic to the ListView
            if (!listViewTopics.Items.ContainsKey(topic))
            {
                listViewTopics.Items.Add(new ListViewItem { Text = topic, Name = topic });
                // Check if the client is connected before subscribing
                if (_mqttClient.IsConnected())
                {
                    _mqttClient.Subscribe(topic);
                } 
                // If the client is not connected, show an error message
                else
                {
                    MessageBox.Show("Client is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            // If the topic is already subscribed to, show an error message
            else
            {
                MessageBox.Show("Topic is already subscribed to", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Gets messages from Client and displays them in the ListView.
        /// </summary>
        /// <param name="topic">The topic to display</param>
        private void DisplayMessagesForTopic(string topic)
        {
            // Get messages for the selected topic
            topicMessages[topic] = _mqttClient.GetMessages(topic);
            // Clear the ListView
            listViewMessages.Items.Clear();

            // Add messages to the ListView
            foreach (Message message in topicMessages[topic])
            {
                // Create a new ListViewItem with the message details
                ListViewItem item = new ListViewItem(new string[] { 
                    message.Timestamp.ToString(), 
                    message.Sender.ToString(), 
                    message.Content.ToString() 
                });
                listViewMessages.Items.Add(item);
            }
        }

        /// <summary>
        /// Sets up the MQTT client with the broker address and subscribes to the MqttChat topics. <br>
        /// If client is already connected then it does nothing.
        /// </summary>
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
            // Subscribe to the message received event
            _mqttClient.MessageReceived += MqttClient_MessageReceived;
            // Subscribe to MqttChat topics by default
            SubscribeToTopic("MqttChat/#");
        }

        /// <summary>
        /// Handles a message received event from the MQTT client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttClient_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            // Add the message to the topicMessages dictionary
            if (!topicMessages.ContainsKey(e.Message_.Topic))
            {
                topicMessages[e.Message_.Topic] = new List<Message>();
            }
            topicMessages[e.Message_.Topic].Add(e.Message_);
            
            // Update the message display if the selected topic is the same as the received topic
            Invoke((MethodInvoker)delegate
            {
                if (listViewTopics.Items.Count > _selectedTopic && MqttTopicMatcher.IsMatch(listViewTopics.Items[_selectedTopic].Text, e.Message_.Topic))
                {
                    DisplayMessagesForTopic(e.Message_.Topic);
                }
            });
        }

        /// <summary>
        /// When the form loads, maximize the form and setup the MQTT client.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Maximize the form on load
            WindowState = FormWindowState.Maximized;
            // Setup the MQTT client
            SetupMqttClient();
        }
    }
}
