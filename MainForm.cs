using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MQTT_Intek
{
    /// <summary>
    /// Class to represent the main form of the application.
    /// </summary>
    public partial class MainForm : Form
    {
        private Client _mqttClient;
        private Dictionary<string, List<Message>> _topicMessages;
        private int _selectedTopicIndex = -1;
        private string _selectedTopicName;

        /// <summary>
        /// Constructor for the main form.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            _topicMessages = new Dictionary<string, List<Message>>();
        }

        /// <summary>
        /// Handles the click event for the subscribe button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubscribe_Click(object sender, EventArgs e)
        {
            // Get the topic from the text box
            string topic = txtNewTopic.Text.Trim();
            // Check if the topic is not empty
            if (!string.IsNullOrEmpty(topic))
            {
                // Subscribe to the topic
                SubscribeToTopic(topic);
                // Clear the text box
                txtNewTopic.Clear();
            }
        }

        /// <summary>
        /// Handle the click event for the change broker button. Opens a new form to change the broker connection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangeBroker_Click(object sender, EventArgs e)
        {
            // Open the broker connection form
            using (BrokerConnectionForm brokerConnectionForm = new BrokerConnectionForm())
            {
                // Save the previous connection status text
                var previousText = connectionStatusLabel.Text;
                // Show the broker connection form
                brokerConnectionForm.ShowDialog();
                // If the user clicked OK and entered a hostname
                if (brokerConnectionForm.DialogResult == DialogResult.OK && brokerConnectionForm.Hostname != "")
                {
                    // Set the connection status label
                    connectionStatusLabel.Text = $"Attempting to connect to {brokerConnectionForm.Hostname}...";

                    var newClient = SetupMqttClient(brokerConnectionForm.Hostname, brokerConnectionForm.Port());

                    // If the client could not connect, show an error message
                    if (newClient == null)
                    {
                        connectionStatusLabel.Text = previousText;
                        MessageBox.Show("Could not connect to the broker", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    // Disconnect the current client
                    _mqttClient.Disconnect();
                    // Set the new client
                    _mqttClient = newClient;

                    // Clear the messages from the ListView
                    listViewMessages.Items.Clear();
                    // Clear the topics from the ListView
                    listViewTopics.Items.Clear();
                    // Clear the topicMessages dictionary
                    _topicMessages.Clear();

                    // Setup the MQTT client with the new broker address and port
                    SetupMqttClient(brokerConnectionForm.Hostname, brokerConnectionForm.Port());   
                }
            }
        }

        /// <summary>
        /// Event handler for the send button click event. Sends a message with the content of the text box to the selected topic.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            // Check if a topic is selected
            if (_selectedTopicIndex != -1)
            {
                // Get the name of the selected topic
                string selectedTopicName = listViewTopics.Items[_selectedTopicIndex].Text;
                // Get the message content
                string messageContent = txtMessage.Text.Trim();
                // Check if the message content is not empty
                if (!string.IsNullOrEmpty(messageContent))
                {
                    // Clear the message text box
                    txtMessage.Clear();
                    // Send the message to the selected topic
                    _mqttClient.SendMessage(messageContent, selectedTopicName);
                }
                // Message content is empty
                else
                {
                    MessageBox.Show("Please enter a message to send.", "No Message Entered", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            // No topic is selected
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
            // Only proceed if a single topic is selected
            if (listViewTopics.SelectedItems.Count == 1)
            {
                // Get the selected topic
                string selectedTopicName = listViewTopics.SelectedItems[0].Text;

                // If the selected topic is the same as the currently selected topic, do nothing
                if (_selectedTopicIndex > listViewTopics.Items.Count && selectedTopicName == listViewTopics.Items[_selectedTopicIndex].Text)
                {
                    return;
                }

                // Set the new selected topic index
                _selectedTopicIndex = listViewTopics.Items.IndexOf(listViewTopics.SelectedItems[0]);
                _selectedTopicName = selectedTopicName;
                // Highlight selected topic
                for (int i = 0; i < listViewTopics.Items.Count; i++)
                {
                    if (i == _selectedTopicIndex)
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
                _topicMessages[topic] = new List<Message>();
                // Check if the client is connected before subscribing
                if (_mqttClient != null && _mqttClient.IsConnected())
                {
                    try
                    {
                        _mqttClient.Subscribe(topic);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Could not subscribe to the topic", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
            // Clear the ListView
            listViewMessages.Items.Clear();

            var wildcardMessages = _topicMessages.Where(x => MqttTopicMatcher.IsMatch(topic, x.Key)).SelectMany(x => x.Value).ToList();
            wildcardMessages.Sort((x, y) => x.Timestamp.CompareTo(y.Timestamp));
            // Add messages to the ListView
            foreach (Message message in wildcardMessages)
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
        /// <param name="hostname"/>The hostname of the broker</param>
        /// <param name="port"/>The port of the broker</param>
        private Client SetupMqttClient(string hostname, int port = 1883)
        {
            try
            {
                // Initialize the client with the broker address
                Client newClient = new Client(hostname, Guid.NewGuid().ToString(), port);

                // If the client could not connect, return null
                if (!newClient.IsConnected())
                {
                    return null;
                }
                // Set the label to show the connection status
                connectionStatusLabel.Text = $"Connected to {hostname}";
                // Subscribe to the message received event
                newClient.MessageReceived += MqttClient_MessageReceived;
                return newClient;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Handles a message received event from the MQTT client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttClient_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            // Add the message to the topicMessages dictionary
            if (!_topicMessages.ContainsKey(e.Message_.Topic))
            {
                _topicMessages[e.Message_.Topic] = new List<Message>();
            }
            _topicMessages[e.Message_.Topic].Add(e.Message_);
            
            // Update the message display if the selected topic is the same as the received topic
            Invoke((MethodInvoker)delegate
            {
                if (listViewTopics.Items.Count > _selectedTopicIndex && MqttTopicMatcher.IsMatch(_selectedTopicName, e.Message_.Topic))
                {
                    DisplayMessagesForTopic(_selectedTopicName);
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
            // Setup the MQTT client with the default broker address
            _mqttClient = SetupMqttClient("broker.hivemq.com");
            if (_mqttClient == null)
            {
                MessageBox.Show("Could not connect to the broker", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            // Subscribe to MqttChat topics by default
            SubscribeToTopic("MqttChat/#");
        }
    }
}
