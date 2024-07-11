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
            this._txtNewTopic = new System.Windows.Forms.TextBox();
            this._btnSubscribe = new System.Windows.Forms.Button();
            this._listViewTopics = new System.Windows.Forms.ListView();
            this._listViewMessages = new System.Windows.Forms.ListView();
            this.timestampHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.senderHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contentHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._txtMessage = new System.Windows.Forms.TextBox();
            this._btnSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtNewTopic
            // 
            this._txtNewTopic.Location = new System.Drawing.Point(12, 7);
            this._txtNewTopic.Name = "txtNewTopic";
            this._txtNewTopic.Size = new System.Drawing.Size(200, 26);
            this._txtNewTopic.TabIndex = 0;
            // 
            // btnSubscribe
            // 
            this._btnSubscribe.Location = new System.Drawing.Point(218, 7);
            this._btnSubscribe.Name = "btnSubscribe";
            this._btnSubscribe.Size = new System.Drawing.Size(100, 26);
            this._btnSubscribe.TabIndex = 1;
            this._btnSubscribe.Text = "Subscribe";
            this._btnSubscribe.Click += new System.EventHandler(this.btnSubscribe_Click);
            // 
            // listViewTopics
            // 
            this._listViewTopics.HideSelection = false;
            this._listViewTopics.Location = new System.Drawing.Point(12, 40);
            this._listViewTopics.Name = "listViewTopics";
            this._listViewTopics.Size = new System.Drawing.Size(306, 200);
            this._listViewTopics.TabIndex = 2;
            this._listViewTopics.UseCompatibleStateImageBehavior = false;
            this._listViewTopics.View = System.Windows.Forms.View.List;
            this._listViewTopics.Click += new System.EventHandler(this.listViewTopics_Click);
            // 
            // listViewMessages
            // 
            this._listViewMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.timestampHeader,
            this.senderHeader,
            this.contentHeader});
            this._listViewMessages.HideSelection = false;
            this._listViewMessages.Location = new System.Drawing.Point(327, 40);
            this._listViewMessages.Name = "listViewMessages";
            this._listViewMessages.Size = new System.Drawing.Size(605, 200);
            this._listViewMessages.TabIndex = 3;
            this._listViewMessages.UseCompatibleStateImageBehavior = false;
            this._listViewMessages.View = System.Windows.Forms.View.Details;
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
            this._txtMessage.Location = new System.Drawing.Point(12, 250);
            this._txtMessage.Name = "txtMessage";
            this._txtMessage.Size = new System.Drawing.Size(811, 26);
            this._txtMessage.TabIndex = 0;
            // 
            // btnSend
            // 
            this._btnSend.Location = new System.Drawing.Point(829, 250);
            this._btnSend.Name = "btnSend";
            this._btnSend.Size = new System.Drawing.Size(75, 26);
            this._btnSend.TabIndex = 0;
            this._btnSend.Text = "Send";
            this._btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1296, 479);
            this.Controls.Add(this._txtNewTopic);
            this.Controls.Add(this._btnSubscribe);
            this.Controls.Add(this._listViewTopics);
            this.Controls.Add(this._listViewMessages);
            this.Controls.Add(this._txtMessage);
            this.Controls.Add(this._btnSend);
            this.Name = "MainForm";
            this.Text = "MQTT Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ColumnHeader timestampHeader;
        private ColumnHeader senderHeader;
        private ColumnHeader contentHeader;
    }
}

