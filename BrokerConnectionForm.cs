using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MQTT_Intek
{
    public partial class BrokerConnectionForm : Form
    {
        public BrokerConnectionForm()
        {
            InitializeComponent();
        }
        public string Hostname => hostNameInput.Text;

        /// <summary>
        /// Checks if the port input is a valid number and returns it.
        /// </summary>
        /// <returns>The parsed port input or 1883 if it could not be parsed</returns>
        public int Port()
        {
            // Try to parse the port input
            if (int.TryParse(portInput.Text, out int port)) 
            {
                return port;
            }
            // If the port input could not be parsed, return the default port
            else
            {
                return 1883;
            }
        }

        /// <summary>
        /// Event handler for when a key is pressed in the port input box. Controls that only digits can be entered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void portInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is a digit or a control key (like Backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Suppress the key press event
            }
        }
    }
}
