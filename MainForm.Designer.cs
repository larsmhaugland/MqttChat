using System.Windows.Forms;
using System;

namespace MQTT_Intek
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtNewTopic = new System.Windows.Forms.TextBox();
            this.btnSubscribe = new System.Windows.Forms.Button();
            this.listViewTopics = new System.Windows.Forms.ListView();
            this.listViewMessages = new System.Windows.Forms.ListView();
            this.timestampHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.senderHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contentHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.changeBroker = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtNewTopic
            // 
            this.txtNewTopic.Location = new System.Drawing.Point(12, 7);
            this.txtNewTopic.Name = "txtNewTopic";
            this.txtNewTopic.Size = new System.Drawing.Size(200, 26);
            this.txtNewTopic.TabIndex = 0;
            // 
            // btnSubscribe
            // 
            this.btnSubscribe.Location = new System.Drawing.Point(218, 7);
            this.btnSubscribe.Name = "btnSubscribe";
            this.btnSubscribe.Size = new System.Drawing.Size(100, 26);
            this.btnSubscribe.TabIndex = 1;
            this.btnSubscribe.Text = "Subscribe";
            this.btnSubscribe.Click += new System.EventHandler(this.btnSubscribe_Click);
            // 
            // listViewTopics
            // 
            this.listViewTopics.HideSelection = false;
            this.listViewTopics.Location = new System.Drawing.Point(12, 40);
            this.listViewTopics.Name = "listViewTopics";
            this.listViewTopics.Size = new System.Drawing.Size(306, 200);
            this.listViewTopics.TabIndex = 2;
            this.listViewTopics.UseCompatibleStateImageBehavior = false;
            this.listViewTopics.View = System.Windows.Forms.View.List;
            this.listViewTopics.Click += new System.EventHandler(this.listViewTopics_Click);
            // 
            // listViewMessages
            // 
            this.listViewMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.timestampHeader,
            this.senderHeader,
            this.contentHeader});
            this.listViewMessages.HideSelection = false;
            this.listViewMessages.Location = new System.Drawing.Point(327, 40);
            this.listViewMessages.Name = "listViewMessages";
            this.listViewMessages.Size = new System.Drawing.Size(605, 200);
            this.listViewMessages.TabIndex = 3;
            this.listViewMessages.UseCompatibleStateImageBehavior = false;
            this.listViewMessages.View = System.Windows.Forms.View.Details;
            // 
            // timestampHeader
            // 
            this.timestampHeader.Text = "Timestamp";
            this.timestampHeader.Width = 150;
            // 
            // senderHeader
            // 
            this.senderHeader.Text = "Sender";
            this.senderHeader.Width = 150;
            // 
            // contentHeader
            // 
            this.contentHeader.Text = "Content";
            this.contentHeader.Width = 300;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(12, 250);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(811, 26);
            this.txtMessage.TabIndex = 0;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(829, 250);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 26);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // changeBroker
            // 
            this.changeBroker.Location = new System.Drawing.Point(802, 7);
            this.changeBroker.Name = "changeBroker";
            this.changeBroker.Size = new System.Drawing.Size(85, 26);
            this.changeBroker.TabIndex = 4;
            this.changeBroker.Text = "Change";
            this.changeBroker.UseVisualStyleBackColor = true;
            this.changeBroker.Click += new System.EventHandler(this.btnChangeBroker_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(324, 10);
            this.label1.Name = "ConnectionStatus";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1296, 479);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.changeBroker);
            this.Controls.Add(this.txtNewTopic);
            this.Controls.Add(this.btnSubscribe);
            this.Controls.Add(this.listViewTopics);
            this.Controls.Add(this.listViewMessages);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnSend);
            this.Name = "MainForm";
            this.Text = "MQTT Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TextBox txtNewTopic;
        private Button btnSubscribe;
        private ListView listViewTopics;
        private ListView listViewMessages;
        private TextBox txtMessage;
        private Button btnSend;
        private ColumnHeader timestampHeader;
        private ColumnHeader senderHeader;
        private ColumnHeader contentHeader;
        private Button changeBroker;
        private Label label1;
    }
}

