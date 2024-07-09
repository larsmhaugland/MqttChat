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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtNewTopic = new System.Windows.Forms.TextBox();
            this.btnSubscribe = new System.Windows.Forms.Button();
            this.listViewTopics = new System.Windows.Forms.ListView();
            this.listViewMessages = new System.Windows.Forms.ListView();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtNewTopic
            // 
            this.txtNewTopic.Location = new System.Drawing.Point(12, 12);
            this.txtNewTopic.Name = "txtNewTopic";
            this.txtNewTopic.Size = new System.Drawing.Size(200, 26);
            this.txtNewTopic.TabIndex = 0;
            // 
            // btnSubscribe
            // 
            this.btnSubscribe.Location = new System.Drawing.Point(220, 11);
            this.btnSubscribe.Name = "btnSubscribe";
            this.btnSubscribe.Size = new System.Drawing.Size(100, 28);
            this.btnSubscribe.TabIndex = 1;
            this.btnSubscribe.Text = "Subscribe";
            this.btnSubscribe.Click += new System.EventHandler(this.btnSubscribe_Click);
            // 
            // listViewTopics
            // 
            this.listViewTopics.HideSelection = false;
            this.listViewTopics.Location = new System.Drawing.Point(12, 40);
            this.listViewTopics.Name = "listViewTopics";
            this.listViewTopics.Size = new System.Drawing.Size(280, 200);
            this.listViewTopics.TabIndex = 2;
            this.listViewTopics.UseCompatibleStateImageBehavior = false;
            this.listViewTopics.View = System.Windows.Forms.View.List;
            this.listViewTopics.Click += new System.EventHandler(this.listViewTopics_Click);
            // 
            // listViewMessages
            // 
            this.listViewMessages.HideSelection = false;
            this.listViewMessages.Location = new System.Drawing.Point(300, 40);
            this.listViewMessages.Name = "listViewMessages";
            this.listViewMessages.Size = new System.Drawing.Size(500, 200);
            this.listViewMessages.TabIndex = 3;
            this.listViewMessages.UseCompatibleStateImageBehavior = false;
            this.listViewMessages.View = System.Windows.Forms.View.Details;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(300, 250);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(400, 26);
            this.txtMessage.TabIndex = 0;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(710, 248);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1296, 479);
            this.Controls.Add(this.txtNewTopic);
            this.Controls.Add(this.btnSubscribe);
            this.Controls.Add(this.listViewTopics);
            this.Controls.Add(this.listViewMessages);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnSend);
            this.Name = "MainForm";
            this.Text = "MQTT Client";

        }

        #endregion
    }
}

